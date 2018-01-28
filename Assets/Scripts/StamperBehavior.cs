using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StamperBehavior : MonoBehaviour
{
    public GameObject StamperStamp;
    public AudioClip StampAudio;

	// Use this for initialization
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = StampAudio;
	}
	
	// Update is called once per frame
    void Update()
    {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        var letterEntity = collider.GetComponent<LetterEntity>();
        if (letterEntity == null)
        {
            return;
        }
                    
        var newStamperStamp = Instantiate(StamperStamp, collider.transform);
        newStamperStamp.transform.position = this.transform.position;
        newStamperStamp.transform.rotation = this.transform.rotation;
        letterEntity.IsStamped = true;
        GetComponent<AudioSource>().Play();
    }
}
