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
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<ScoreEvent>(ChangeScore);
	}

	private void ChangeScore(ScoreEvent scoreEvent)
	{
		_score += scoreEvent.Points;
		_textComponent.text = _score.ToString();
	}
}
