using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WaveChangeDisplay : MonoBehaviour
    {
        private Text _textComponent;

        public void Start()
        {
            _textComponent = GetComponent<Text>();
            _textComponent.text = "Wave 1";

            Events.instance.AddListener<WaveChangeEvent>(DisplayWaveChange);
        }

        public void OnDestroy()
        {
            Events.instance.RemoveListener<WaveChangeEvent>(DisplayWaveChange);
        }
        
        private void DisplayWaveChange(WaveChangeEvent waveChangeEvent)
        {
            _textComponent.text = string.Format("Wave {0}", waveChangeEvent.Wave);
        }
    }
}