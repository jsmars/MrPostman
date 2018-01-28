using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using VRTK.UnityEventHelper;

public class RestartScene : MonoBehaviour {

	private VRTK_Button_UnityEvents _buttonEvents;
	private float _disabledTime = 5;

	public void Start()
	{
		_buttonEvents = GetComponent<VRTK_Button_UnityEvents>() ?? gameObject.AddComponent<VRTK_Button_UnityEvents>();
		_buttonEvents.OnPushed.AddListener(HandlePush);
	}

	private void HandlePush(object sender, Control3DEventArgs e)
	{
		if(_disabledTime > 0)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public void Update()
	{
		if(_disabledTime > 0)
		{
			_disabledTime -= Time.deltaTime;
		}
	}
}
