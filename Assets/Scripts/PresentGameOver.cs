using UnityEngine;

public class PresentGameOver : MonoBehaviour
{
	public GameObject GameOverText;

	public void Start()
	{
		Events.instance.AddListener<GameOverEvent>(ActivateGameOver);
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<GameOverEvent>(ActivateGameOver);
	}

	private void ActivateGameOver(GameOverEvent e)
	{
		GameOverText.SetActive(true);
	}
}
