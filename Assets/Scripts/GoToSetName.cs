using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using VRTK.UnityEventHelper;

public class GoToSetName : MonoBehaviour
{
	private VRTK_Button_UnityEvents _buttonEvents;

	public void Start()
	{
		_buttonEvents = GetComponent<VRTK_Button_UnityEvents>() ?? gameObject.AddComponent<VRTK_Button_UnityEvents>();
		_buttonEvents.OnPushed.AddListener(HandlePush);
	}

	private void HandlePush(object sender, Control3DEventArgs e)
	{
		SceneManager.LoadScene("set_name");
	}
}
