using UnityEngine;

public class ScoreBarCounter : MonoBehaviour
{
	public GameObject Bar;
	public float Decrease = 0.02f;
	private float _score = 1;
	public float LetterIncrease = 0.1f;
	private bool _gameOver;

	public void Start()
	{
		Events.instance.AddListener<ScoreEvent>(ChangeBar);
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<ScoreEvent>(ChangeBar);
	}

	public void Update()
	{
        var waveIncr = 1 + ConveyorBehavior.CurrentWave / 25f;
        var decrSpeed = -Decrease * waveIncr;
        Debug.Log("Decrease speed: " + decrSpeed);

        ChangeBar(decrSpeed * Time.deltaTime);

		if (_score <= 0 && !_gameOver)
		{
			_gameOver = true;
			Events.instance.Raise(new GameOverEvent());
		}
	}

	private void ChangeBar(ScoreEvent scoreEvent)
	{
		ChangeBar(LetterIncrease);
	}

	private void ChangeBar(float change)
	{
		_score = Mathf.Clamp(_score + change, 0, 1);
		var currentScale = Bar.transform.localScale;
		Bar.transform.localScale = new Vector3(_score, currentScale.y, currentScale.z);
	}
}
