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
	}

	public void OnDestroy()
	{
		_vrtkControllerEvents.TouchpadPressed -= Restart;
	}

	private void Restart(object sender, ControllerInteractionEventArgs e)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
