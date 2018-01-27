using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class RestartScene : MonoBehaviour
{
	private VRTK_ControllerEvents _vrtkControllerEvents;

	public void Start()
	{
		_vrtkControllerEvents = GetComponent<VRTK_ControllerEvents>();
		_vrtkControllerEvents.TouchpadPressed += Restart;
        _vrtkControllerEvents.ButtonTwoPressed += ResetView;
	}

	public void OnDestroy()
	{
		_vrtkControllerEvents.TouchpadPressed -= Restart;
	}

	private void Restart(object sender, ControllerInteractionEventArgs e)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetView(object sender, ControllerInteractionEventArgs e)
    {
        UnityEngine.XR.InputTracking.Recenter();
    }
}
