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
		Events.instance.AddListener<ScoreEvent>(ChangeScore);
		Events.instance.AddListener<GameOverEvent>(PostScore);
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<ScoreEvent>(ChangeScore);
		Events.instance.RemoveListener<GameOverEvent>(PostScore);
	}

	private void ChangeScore(ScoreEvent scoreEvent)
	{
		_score += scoreEvent.Points;
		_textComponent.text = _score.ToString();
	}

	private void PostScore(GameOverEvent e)
	{
		Events.instance.Raise(new PostScoreEvent(_score));
	}
}
