using System.Collections.Generic;
using UnityEngine;

public class BinCollected : MonoBehaviour
{
	public ParticleSystem Particles;
	private readonly HashSet<GameObject> _binCollected = new HashSet<GameObject>();
    public LetterColor LetterColor;
    public int LetterNumber;

    public void OnTriggerEnter(Collider other)
	{
		if(!HasCollected(other))
		{
            var letter = TryGetLetter(other);
            if (letter != null && letter.TryScore(this))
            {
                //Destroy(other.gameObject);
                Particles.Play();
                _binCollected.Add(other.gameObject);
            }
		}
	}

	private static MakeLetter TryGetLetter(Collider other)
	{
        return other.gameObject.GetComponent<MakeLetter>();
	}

	private bool HasCollected(Collider other)
	{
		return _binCollected.Contains(other.gameObject);
    }

    public override string ToString()
    {
        return "BIN " + LetterColor + " / " + LetterNumber;
    }
}