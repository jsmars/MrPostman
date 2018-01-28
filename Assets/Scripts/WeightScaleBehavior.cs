using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScaleBehavior : MonoBehaviour {

    public GameObject WeightDisplay;
    private TextMesh _weightDisplayText;

	// Use this for initialization
	void Start () {
        _weightDisplayText = WeightDisplay.GetComponent<TextMesh>();
        if (_weightDisplayText == null) 
        {
            throw new Exception("No weight text component");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider) 
    {
        var letterEntity = collider.GetComponent<LetterEntity>();
        if (letterEntity == null) 
        {
            return;
        }
          
        var weight = letterEntity.Weight;
        if (weight < 0.1) 
        {
            weight = 0.2f;
        }

        _weightDisplayText.text = string.Format("{0} kg", weight.ToString());
    }

    void OnTriggerExit(Collider collider) 
    {
        var letterEntity = collider.GetComponent<LetterEntity>();
        if (letterEntity == null)
        {
            return;
        }

        _weightDisplayText.text = string.Format("{0} kg", "0.0");    
    }
}
