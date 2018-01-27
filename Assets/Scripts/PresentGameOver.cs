using UnityEngine;

public class PresentGameOver : MonoBehaviour
{
	public GameObject GameOverText;

	public void Start()
	{
		Events.instance.AddListener<GameOverEvent>(e => GameOverText.SetActive(true));
	}
}
