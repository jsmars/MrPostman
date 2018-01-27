using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision) 
    {
        var isConveyorSubject = collision.collider.GetComponent<LetterEntity>() != null;
        if (!isConveyorSubject)
        {
            return;
        }

        Destroy(collision.collider.gameObject);
    }

}
