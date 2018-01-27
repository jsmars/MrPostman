using System.Collections.Generic;
using UnityEngine;

public class BinCollected : MonoBehaviour
{
	public ParticleSystem Particles;
	private readonly HashSet<GameObject> _binCollected = new HashSet<GameObject>();
	private bool _gameOver;

	public void Start()
	{
		Events.instance.AddListener<GameOverEvent>(StopCollecting);
	}

	private void StopCollecting(GameOverEvent e)
	{
		_gameOver = true;
	}

	public void OnTriggerEnter(Collider other)
	{
		if(!_gameOver && IsLetter(other) && !HasCollected(other))
		{
			//Destroy(other.gameObject);
			Events.instance.Raise(new ScoreEvent(10));
			Particles.Play();
			_binCollected.Add(other.gameObject);
		}
	}

	private static bool IsLetter(Collider other)
	{
		return other.gameObject.name.Contains("Letter");
	}

	private bool HasCollected(Collider other)
	{
		return _binCollected.Contains(other.gameObject);
	}
}