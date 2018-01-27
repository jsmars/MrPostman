using System;
using UnityEngine;

public enum LetterColor { Red, Blue, Yellow }

public static class Helpers
{
	private static readonly int _letterColorCount = Enum.GetValues(typeof(LetterColor)).Length;

	public static Color FromLetterColor(LetterColor color)
	{
		switch (color)
		{
			case LetterColor.Red: return Color.red;
			case LetterColor.Blue: return Color.blue;
			case LetterColor.Yellow: return Color.yellow;
			default:
				Debug.LogError("Missing color for: " + color);
				return Color.black;
		}
	}

	public static void SetStampColor(GameObject obj, LetterColor color)
	{
		var stamp = obj.transform.Find("Stamp");
		if (stamp == null)
			Debug.LogError(obj + " is missing Stamp");
		else
		{
			var stampMat = stamp.GetComponent<Renderer>();
			if (stampMat == null)
				Debug.LogError(obj + " is missing StampMaterial");
			else
				stampMat.material.color = Helpers.FromLetterColor(color);
		}
	}

	public static int LetterColorCount
	{
		get
		{
			return _letterColorCount;
		}
	}
}
