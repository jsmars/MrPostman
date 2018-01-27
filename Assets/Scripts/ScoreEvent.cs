public class ScoreEvent : GameEvent
{
	public float Points { get; private set; }

	public ScoreEvent(float points)
	{
		Points = points;
	}
}