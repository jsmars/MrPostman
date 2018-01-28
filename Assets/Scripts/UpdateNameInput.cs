using UnityEngine;
using UnityEngine.UI;

public class UpdateNameInput : MonoBehaviour
{
	public InputField NameInput;

	public void Start()
	{
		NameInput.text = PlayerName.Name;
	}
}

public static class PlayerName
{
	public static string Name;
}