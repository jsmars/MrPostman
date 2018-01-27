using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadHighscores : MonoBehaviour
{
	public Transform HighscoresPanel;
	public GameObject HighscoreTemplate;
	public Text PersonalBest;
	public Text TotalPlays;

	public void Start()
	{
		Events.instance.AddListener<GameOverEvent>(UpdateScores);
		UpdateScores();
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<GameOverEvent>(UpdateScores);
	}

	private void UpdateScores(GameOverEvent e)
	{
		UpdateScores();
	}

	private void UpdateScores()
	{
		var response = new ScoreResponse(100, 10, new List<Highscore>()
		{
			new Highscore("SvDvorak", 999),
			new Highscore("Erik Lenells", 800),
			new Highscore("Adam Adamsson", 299),
			new Highscore("ItTotallyRocks!!1!", 231),
			new Highscore("AnnoyingMan", 100),
			new Highscore("Man coolio", 49),
			new Highscore("Super McGamingson", 33),
			new Highscore("SuperCoolGuy111", 10),
			new Highscore("!!11Snipah11!!", 9),
			new Highscore("ThisGameSucks", 1)
		});

		foreach (var highscore in response.Highscores)
		{
			var panel = Instantiate(HighscoreTemplate, HighscoresPanel, false);
			panel.GetComponent<HighscoreSetter>().Set(highscore);
		}

		PersonalBest.text = response.PersonalScore.ToString();
		TotalPlays.text = response.TotalPlays.ToString();
	}

	public void Reload()
	{
		foreach (Transform child in HighscoresPanel)
		{
			Destroy(child.gameObject);
		}

		UpdateScores();
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