using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class MusicAnalyzer : MonoBehaviour 
    {
        private AudioSource _audio;
        float[] spectrum = new float[128];
        // Spectrum is multiplayed with volume. To take any values from spectrum, volume should have not zero value
        private float spectrumMultiply = 1;

        public void SetMusic(AudioSource audio)
        {
            _audio = audio;
        }
        
        private void AnalyzeMusic()
        {
            if (AudioListener.volume > 0)
            {                
                _audio.GetOutputData(spectrum, 0);
                spectrumMultiply = 1 / _audio.volume;
            }
        }
    }
}
