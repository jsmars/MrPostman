using System.Collections.Generic;
using UnityEngine;

public class FloorTouched : MonoBehaviour
{
	private readonly HashSet<GameObject> _binCollected = new HashSet<GameObject>();

	public void OnTriggerEnter(Collider other)
	{
		if (IsLetter(other) && !HasCollected(other))
		{
			Events.instance.Raise(new LetterFailEvent());
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

public class LetterFailEvent : GameEvent
{
}