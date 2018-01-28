using UnityEngine;
using UnityEngine.UI;

public class ReadPcName : MonoBehaviour
{
	public InputField NameInput;

	public void Start()
	{
		if (NameInput.text == "")
		{
			NameInput.text = SystemInfo.deviceName;
		}
		else
		{
			NameInput.text = PlayerName.Name;
		}
	}
}

public static class PlayerName
{
	public static string Name;
}