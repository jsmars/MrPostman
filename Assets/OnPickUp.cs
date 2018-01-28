using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using Assets.Scripts;

public class OnPickUp : MonoBehaviour {

    private VRTK_InteractGrab _vrtkControllerEvents;
    private DelayDestroy lastPickup;
    // Use this for initialization
    void Start ()
    {
        _vrtkControllerEvents = GetComponent<VRTK_InteractGrab>();
        _vrtkControllerEvents.ControllerGrabInteractableObject += onGrabbed;
    }
    
	// Update is called once per frame
	void Update ()
    {
        if (lastPickup != null)
            lastPickup.Reset(); // Just reset the last held object, so that one that is held never can dissapear
	}

    void onGrabbed(object sender, ObjectInteractEventArgs e)
    {
        lastPickup = e.target.GetComponent<DelayDestroy>();
    }
}
