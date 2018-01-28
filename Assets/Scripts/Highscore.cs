using System;
using System.Collections.Generic;
using System.Linq;
using jsmars;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using UnityEngine;
using System.Net.NetworkInformation;

namespace jsmars
{
    public enum HighscoreStatus { Downloading, Updated, ServerConnectionError }
    public enum HighscoreSubmitStatus { Idle, Sending }
    public class Highscore
    {
        public bool ENABLE = true;
        public int MessageDisplayCount = 4;
        public string GetName()
        {
			return PlayerName.Name;
        }

        public ulong GetUID()
        {
	        return 0;
        }

        public int ConnectionAttempts = 20;

        public int Version { get; set; }
        public int FetchCount = 10;
        public string GameName { get; private set; }
        public string HighscoreURL { get; private set; }
        public HighscoreStatus Status { get { return status; } private set { status = value; } }
        public HighscoreSubmitStatus SubmitStatus { get { return submitStatus; } private set { submitStatus = value; } }

        HighscoreStatus status = HighscoreStatus.Downloading;
        HighscoreSubmitStatus submitStatus = HighscoreSubmitStatus.Idle;

        // Highscore stats
        public HighscoreEntry StatPersonalBest { get; private set; }
        public string StatPersonalTotalTime { get; private set; }
        public string StatPersonalTotalScore { get; private set; }
        public string StatPersonalTotalPlays { get; private set; }
        public string StatGlobalTotalTime { get; private set; }
        public string StatGlobalTotalScore { get; private set; }
        public string StatGlobalTotalPlays { get; private set; }
        public List<HighscoreEntry> StatHighscoreList { get { return statHighscoreList; } private set { statHighscoreList = value; } }

        List<HighscoreEntry> statHighscoreList = new List<HighscoreEntry>();
        
        public List<OnlineMessage> msgs = new List<OnlineMessage>(); //ensure thread safety on list
        Queue<HighscoreEntry> submissionQue = new Queue<HighscoreEntry>();

        public static Regex CleanName = new Regex("[^a-zA-Z0-9 -]");

        // Threaded
        BackgroundWorker getWorker, sendWorker;
        List<OnlineMessage> msgsThreaded = new List<OnlineMessage>();
        List<HighscoreEntry> hsList = new List<HighscoreEntry>();
        HighscoreEntry submissionEntry, hsPersonal;
        int hsFail = 0;
        string[] hsStats = new string[7];
        bool downloadDone;

	    public event Action DownloadDone;

        #region Instance

        public Highscore(string gamename, int version = 1, string highscoreServerURL = "http://highscores.jsmars.com/")
        {
            if (string.IsNullOrEmpty(gamename) || CleanName.IsMatch(gamename))
                throw new Exception("Game must have a valid name");
            
            GameName = gamename;
            Version = version;
            HighscoreURL = highscoreServerURL;

            // Worker for highscores
            getWorker = new BackgroundWorker();
            getWorker.DoWork += new DoWorkEventHandler(getHighscores);
            getWorker.RunWorkerAsync();
            sendWorker = new BackgroundWorker();
            sendWorker.DoWork += new DoWorkEventHandler(sendHighscores);
            sendWorker.RunWorkerAsync();
        }

        public void Update()
        {
            #region Que submissions

            if (submissionQue.Count > 0 && SubmitStatus == HighscoreSubmitStatus.Idle)
            {
                submissionEntry = submissionQue.Dequeue();
                SubmitStatus = HighscoreSubmitStatus.Sending;
            }

            #endregion

            #region Stats threading transfer

            if (downloadDone)
            {
                StatHighscoreList.Clear();
                StatHighscoreList.AddRange(hsList);

                msgs.Clear();
                msgs.AddRange(msgsThreaded);

                StatPersonalBest = hsPersonal;

                if (hsStats.Length >= 7)
                {
                    StatPersonalTotalTime = timeString(hsStats[1]);
                    StatPersonalTotalScore = hsStats[2];
                    StatPersonalTotalPlays = hsStats[3];
                    StatGlobalTotalTime = timeString(hsStats[4]);
                    StatGlobalTotalScore = hsStats[5];
                    StatGlobalTotalPlays = hsStats[6];
                }

                Status = HighscoreStatus.Updated;
                downloadDone = false;

	            if (DownloadDone != null)
	            {
		            DownloadDone();
	            }
            }

            #endregion

            var count = Math.Min(MessageDisplayCount, msgs.Count);
            for (int i = 0; i < count; i++)
                if (Input.GetKeyDown(KeyCode.F3 + i))
                    Process.Start(msgs[i].Link);
        }

        #endregion

        #region Submit

        public void Refresh()
        {
            if (Status != HighscoreStatus.Downloading)
                Status = HighscoreStatus.Downloading;
        }

        public void SubmitScore(double score, TimeSpan time, int mode = 0, int attempts = 0, string customData = "", string identifier = null)
        {
            if (identifier == null)
                identifier = GetFirstMacAdress();
            submissionQue.Enqueue(new HighscoreEntry(GetName(), GetUID(), score, time, mode, attempts, customData, identifier));
        }
        public void SubmitScore(string name, double score, TimeSpan time, int mode = 0, int attempts = 0, string customData = "", string identifier = null)
        {
            if (identifier == null)
                identifier = GetFirstMacAdress();
            submissionQue.Enqueue(new HighscoreEntry(name, GetUID(), score, time, mode, attempts, customData, identifier));
        }

        public static string GetFirstMacAdress()
        {
            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();
        }

        #endregion

        #region Helpers

        private string timeString(string str)
        {
            float f;
            if (float.TryParse(str, out f))
            {
                var ts = TimeSpan.FromSeconds(f);
                var output = string.Format("{0}h {1}m {2}s", ts.Hours, ts.Minutes, ts.Seconds);
                if (ts.Days > 0)
                    output = string.Format("{0}d {1}", ts.Days, output);
                return output;
            }
            return str;
        }

        private HighscoreEntry entryFromArray(int rank, string[] data)
        {
            int version, mode, id, timetotal, attempts;
            ulong uid;
            string name, customData;
            double score;
            TimeSpan time;
            DateTime date;
            name = data[0];
            customData = data[5];
            if (!int.TryParse(data[3], out version) ||
                !int.TryParse(data[4], out mode) ||
                !int.TryParse(data[8], out id) ||
                !ulong.TryParse(data[9], out uid) ||
                !int.TryParse(data[6], out timetotal) ||
                !int.TryParse(data[7], out attempts) ||
                !double.TryParse(data[1], out score) ||
                !DateTime.TryParse(data[2], out date))
                throw new Exception("Error parsing highscore data");
            time = TimeSpan.FromMilliseconds(timetotal);

            return new HighscoreEntry(rank, name, uid, score, time, date, version, mode, id, attempts, customData, null, null);
        }

        #endregion

        #region threaded

        void getHighscores(object sender, DoWorkEventArgs e)
        {
            if (!ENABLE) return;
            while (true)
            {
                try
                {
                    if (Status == HighscoreStatus.Downloading)
                    {
                        var STARTRANK = 1; //TODO: Add as php parameter for custom lists
                        var url = string.Format("{0}index.php?showscores={1}&getlist&limit={2}&max", HighscoreURL, GameName, FetchCount);
                        var hs = GetWebResponse(url);
                        string[] lines = hs.Split('\n');
                        int i = 1;
                        hsList.Clear();
                        foreach (var line in lines)
                        {
                            string[] data = line.Split('\t');
                            if (data.Length >= 9)
                                hsList.Add(entryFromArray(STARTRANK++, data));
                        }

                        url = string.Format("{0}index.php?showscores={1}&getlist&best={2}", HighscoreURL, GameName, GetName());
                        hs = GetWebResponse(url);
                        lines = hs.Split('\n');
                        foreach (var line in lines)
                        {
                            string[] data = line.Split('\t');
                            if (data.Length > 5)
                            {
                                hsPersonal = entryFromArray(0, data);
                                break;
                            }
                        }

                        url = string.Format("{0}index.php?stats={1}&name={2}&version={3}", HighscoreURL, GameName, GetName(), Version);
                        hs = GetWebResponse(url);
                        lines = hs.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
                        lines[0] = lines[0].Replace("\t", " ");
                        for (int x = 0; x <= 6; x++)
                            lines[x] = lines[x].Replace("\t", "");

                        for (int x = 1; x <= 6; x++)
                        {
                            int num;
                            if (!int.TryParse(lines[x], out num))
                                lines[x] = "...";
                        }
                        hsStats = lines;

                        // Messages
                        msgsThreaded.Clear();
                        if (lines.Length >= 8)
                        {
                            lines = lines[7].Split('\t');
                            int msgs = lines.Length / 5;
                            for (int x = 0; x + 4 <= lines.Length; x += 5)
                                msgsThreaded.Add(new OnlineMessage(lines[x], lines[x + 1], lines[x + 2], lines[x + 3], lines[x + 4]));
                        }

                        hsFail = 0;
                        downloadDone = true;
                    }

                }
                catch (Exception exc)
                {
                    hsFail++;
                    if (hsFail > ConnectionAttempts)
                        Status = HighscoreStatus.ServerConnectionError;
                }


                System.Threading.Thread.Sleep(150);
            }
        }

        void sendHighscores(object sender, DoWorkEventArgs e)
        {
            if (!ENABLE) return;
            while (true)
            {
                try
                {
                    if (SubmitStatus == HighscoreSubmitStatus.Sending)
                    {
                        //var oldTimeStr = t.Hours.ToString("00") + t.Minutes.ToString("00") + t.Seconds.ToString("00"); // Old games just had seconds, keep it to not break them
                        var url = string.Concat(HighscoreURL,
                                    "index.php?",
                                    "insertscore=", GameName,
                                    "&version=", Version,
                                    "&mode=", submissionEntry.Mode,
                                    "&name=", submissionEntry.Name,
                                    "&score=", submissionEntry.Score,
                                    "&custom=", submissionEntry.CustomData,
                                    "&timetotal=", submissionEntry.Time.TotalMilliseconds,
                                    "&attempts=", submissionEntry.Attempts,
                                    "&identifier=", submissionEntry.Identifier);
                        GetWebResponse(url);
                        SubmitStatus = HighscoreSubmitStatus.Idle;
                        Status = HighscoreStatus.Downloading;
						Refresh();
                    }
                }
                catch (Exception exc)
                {

                }

                System.Threading.Thread.Sleep(150);
            }
        }

        public static string GetWebResponse(string url, int timoutMs = 1800)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = timoutMs;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        #endregion

    }

    public struct HighscoreEntry
    {
        public int Rank { get; private set; }
        public string Name { get; private set; }
        public double Score { get; private set; }
        public TimeSpan Time { get; private set; }
        public DateTime Date { get; private set; }
        public int Version { get; private set; }
        public int Mode { get; private set; }
        public int Attempts { get; private set; }
        public int ID { get; private set; }
        public string CustomData { get; private set; }
        public string IP { get; private set; }
        public string Identifier { get; private set; }
        public ulong UID { get; private set; }

        public HighscoreEntry(int rank, string name, ulong uid, double score, TimeSpan time, DateTime date, int version, int mode, int id, int attempts, string customData, string ip, string identifier)
        {
            Rank = rank; Name = name; UID = uid; Score = score; Time = time; Date = date; Version = version; Mode = mode; ID = id; Attempts = attempts; CustomData = customData; Identifier = identifier; IP = ip;
        }
        public HighscoreEntry(string name, ulong uid, double score, TimeSpan time, int mode, int attempts, string customData, string identifier)
        {
            Rank = -1; Date = DateTime.Now; ID = -1; Version = -1; Identifier = null; IP = null;
            Name = name; UID = uid; Score = score; Time = time; Mode = mode; Attempts = attempts; CustomData = customData; Identifier = identifier;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} {2}", Rank, Name, Score);
        }
    }

    public class OnlineMessage
    {
        public int Version { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string Link { get; private set; }
        public string Tag { get; private set; }

        public OnlineMessage(string version, string title, string message, string link, string tag)
        {
            int i;
            if (int.TryParse(version, out i))
                Version = i;
            Title = title;
            Message = message;
            Link = link;
            Tag = tag;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", Version, Title, Message, Link, Tag);
        }
    }
}
