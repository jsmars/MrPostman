using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour {

    public AudioClip Burn;

	// Use this for initialization
	void Start () 
    {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = Burn;
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

        GetComponent<AudioSource>().Play();
        Destroy(collision.collider.gameObject);
    }

}
