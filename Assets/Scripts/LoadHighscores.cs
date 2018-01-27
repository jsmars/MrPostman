using System.Collections.Generic;
using UnityEngine;

public class LoadHighscores : MonoBehaviour
{
	public Transform HighscoresPanel;
	public GameObject HighscoreTemplate;

	public void Start()
	{
		var highscores = new List<Highscore>()
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
		};

		foreach (var highscore in highscores)
		{
			var panel = Instantiate(HighscoreTemplate, HighscoresPanel, false);
			panel.GetComponent<HighscoreSetter>().Set(highscore);
		}
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