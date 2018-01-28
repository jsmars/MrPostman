using System;
using System.Collections.Generic;
using System.Linq;
using jsmars;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHighscores : MonoBehaviour
{
	public Transform HighscoresPanel;
	public GameObject HighscoreTemplate;
	public Text Player;
	public Text PersonalBest;
	public Text PersonalPlays;
	public Text TotalPlays;
	public Text Message;
	private jsmars.Highscore _highscore;
	private int _currentCount;

	public void Start()
	{
		if (string.IsNullOrEmpty(PlayerName.Name))
		{
			PlayerName.Name = SystemInfo.deviceName;
		}

		Events.instance.AddListener<PostScoreEvent>(UpdateScores);
		_highscore = new jsmars.Highscore("MrPostman", 1);
		_highscore.DownloadDone += DownloadDone;
		_highscore.Refresh();
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<PostScoreEvent>(UpdateScores);
		_highscore.DownloadDone -= DownloadDone;
	}

	private void UpdateScores(PostScoreEvent postScoreEvent)
	{
		_highscore.SubmitScore(PlayerName.Name, postScoreEvent.Score, new TimeSpan(), 0, 1);
	}

	public void Update()
	{
		_highscore.Update();
	}

	public void DownloadDone()
	{
		UpdateList(_highscore.StatHighscoreList);

		Player.text = PlayerName.Name;

		PersonalBest.text = _highscore.StatPersonalBest.Score.ToString();
		PersonalPlays.text = _highscore.StatPersonalTotalPlays;
		TotalPlays.text = _highscore.StatGlobalTotalPlays;
		Message.text = string.Join("\n", _highscore.msgs.Select(x => x.Title).ToArray());
	}

	private void UpdateList(List<HighscoreEntry> highscores)
	{
		foreach (Transform child in HighscoresPanel)
		{
			Destroy(child.gameObject);
		}

		foreach (var highscore in highscores)
		{
			var panel = Instantiate(HighscoreTemplate, HighscoresPanel, false);
			panel.GetComponent<HighscoreSetter>().Set(highscore);
		}
	}
}

public class ScoreResponse
{
	public float PersonalScore { get; private set; }
	public int TotalPlays { get; private set; }
	public List<Highscore> Highscores { get; private set; }

	public ScoreResponse(float personalScore, int totalPlays, List<Highscore> scores)
	{
		PersonalScore = personalScore;
		TotalPlays = totalPlays;
		Highscores = scores;
	}
}

public class Highscore
{
	public string Name { get; private set; }
	public int Score { get; private set; }

	public Highscore(string name, int score)
	{
		Score = score;
		Name = name;
	}
}