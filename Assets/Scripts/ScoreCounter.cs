using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
	private Text _textComponent;
	private float _score = 0;

	public void Start()
	{
		_textComponent = GetComponent<Text>();
		_textComponent.text = "0";
		Events.instance.AddListener<ScoreEvent>(e => ChangeScore(e.Points));
	}

	private void ChangeScore(float points)
	{
		_score += points;
		_textComponent.text = _score.ToString();
	}
}
