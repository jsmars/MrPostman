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
    public bool IsStamped;
    public LetterTypeEnum LetterType;


    public bool Used { get; private set; }

    // Use this for initialization
    void Start () 
    {
        var letter = gameObject.GetComponent<LetterEntity>();
        Helpers.SetStampColor(gameObject, LetterColor);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool TryScore(BinEntity bin)
    {
        if (Used || bin.LetterType != LetterType)
            return false;

        switch (LetterType)
        {
            case LetterTypeEnum.Letter:
                if (LetterColor != bin.LetterColor)
                    return false;
                break;

            case LetterTypeEnum.Package:
                if (bin.WeightLimitGreater && Weight < bin.WeightLimit || !bin.WeightLimitGreater && Weight > bin.WeightLimit)
                    return false;
                break;

            case LetterTypeEnum.Stamped:
                if (!IsStamped)
                    return false;
                break;

            case LetterTypeEnum.Numbered:
                if (LetterNumber != bin.LetterNumber)
                    return false;
                break;
        }

        Used = true;
        Events.instance.Raise(new ScoreEvent(Score));
        Debug.Log("Scored letter: " + ToString());
        return true;
    }

    public override string ToString()
    {
        return "LETTER " + LetterColor + " / " + LetterNumber;
    }
}
