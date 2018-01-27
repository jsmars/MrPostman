﻿using UnityEngine;

public class ScoreBarCounter : MonoBehaviour
{
	public GameObject Bar;
	public float Decrease;
	private float _score = 1;
	public float LetterIncrease = 0.1f;

	public void Start()
	{
		Events.instance.AddListener<ScoreEvent>(e => ChangeBar(LetterIncrease));
	}

	public void Update()
	{
		ChangeBar(-Decrease * Time.deltaTime);

		if (_score <= 0)
		{
			Events.instance.Raise(new GameOverEvent());
		}
	}

	private void ChangeBar(float change)
	{
		_score = Mathf.Clamp(_score + change, 0, 1);
		var currentScale = Bar.transform.localScale;
		Bar.transform.localScale = new Vector3(_score, currentScale.y, currentScale.z);
	}
}