namespace Assets.Scripts
{
    public class WaveChangeEvent : GameEvent
    {
        public int Wave { get; private set; }
        public WaveChangeEvent(int wave) 
        {
            Wave = wave;
        }
    }
}
