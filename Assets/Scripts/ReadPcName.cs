using UnityEngine;
using UnityEngine.UI;

public class ReadPcName : MonoBehaviour
{
	public InputField NameInput;

	public void Start()
	{
		NameInput.text = SystemInfo.deviceName;
	}
}

public static class PlayerName
{
	public static string Name;
}