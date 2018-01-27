using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
	private Text _textComponent;
	private float _score = 0;

	public void Start()
	{
		_textComponent = GetComponent<Text>();
		_textComponent.text = "";
		Events.instance.AddListener<ScoreEvent>(e => AddScore(e.Points));
		Events.instance.AddListener<LetterFailEvent>(e => AddScore(-50));
	}

	private void AddScore(float points)
	{
		_score += points;
		_textComponent.text = _score.ToString();
	}
}
