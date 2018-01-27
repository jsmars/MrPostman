using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;
using VRTK.SecondaryControllerGrabActions;
using Assets.Scripts.Enums;

public class LetterEntity : MonoBehaviour {
	public List<GameObject> Models;
	public int Score;
    public LetterColor LetterColor;
    public int LetterNumber;
    public float Weight;
    public bool NeedsVAT;
    public LetterTypeEnum LetterType;


    public bool Used { get; private set; }

    // Use this for initialization
    void Start () {


        Helpers.SetStampColor(gameObject, LetterColor);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool TryScore(BinEntity bin)
    {
        if (Used)
            return false;

        if (LetterColor == bin.LetterColor || (LetterNumber > 0 && LetterNumber == bin.LetterNumber))
        {
            Used = true;
            Events.instance.Raise(new ScoreEvent(Score));
            Debug.Log("Scored letter: " + ToString());
            return true;
        }
        else
        {
            Debug.Log("Wrong bin: (" + this + ") tried to go in (" + bin + ")");
            return false;
        }
    }

    public override string ToString()
    {
        return "LETTER " + LetterColor + " / " + LetterNumber;
    }
}
