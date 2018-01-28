using jsmars;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreSetter : MonoBehaviour
{
	public Text Name;
	public Text Score;

	public void Set(HighscoreEntry highscore)
	{
		Name.text = highscore.Name;
		Score.text = highscore.Score.ToString();
	}
}
