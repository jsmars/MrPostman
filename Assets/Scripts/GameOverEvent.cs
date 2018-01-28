public class GameOverEvent : GameEvent
{
}

public class PostScoreEvent : GameEvent
{
	public float Score { get; private set; }

	public PostScoreEvent(float score)
	{
		Score = score;
	}
}