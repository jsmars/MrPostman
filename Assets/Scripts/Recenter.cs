using UnityEngine;
using VRTK;

public class Recenter : MonoBehaviour
{
	private VRTK_ControllerEvents _vrtkControllerEvents;

	public void Start()
	{
		_vrtkControllerEvents = GetComponent<VRTK_ControllerEvents>();
        _vrtkControllerEvents.ButtonTwoPressed += ResetView;
	}

	public void OnDestroy()
	{
        _vrtkControllerEvents.ButtonTwoPressed -= ResetView;
	}

    private void ResetView(object sender, ControllerInteractionEventArgs e)
    {
        UnityEngine.XR.InputTracking.Recenter();
    }
}
