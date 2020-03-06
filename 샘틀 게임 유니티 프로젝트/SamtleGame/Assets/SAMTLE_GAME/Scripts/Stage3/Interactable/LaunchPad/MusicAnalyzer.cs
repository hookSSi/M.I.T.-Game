using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class MusicAnalyzer : MonoBehaviour 
    {
        [Header("오디오 분석기 정보"), Space(20)]
        [SerializeField] private AudioSource _audio;
        [SerializeField] private FFTWindow _fftWindow;

        private float[] _spectrum = new float[128];
        // Spectrum is multiplayed with volume. To take any values from spectrum, volume should have not zero value
        private float spectrumMultiply = 1f;


        public void SetMusic(AudioSource audio)
        {
            _audio = audio;
        }
        
        public void AnalyzeMusic()
        {
            _audio.GetSpectrumData(_spectrum, 0, _fftWindow);
        }

        public float[] GetSpectrum() { return _spectrum; }

        // void Update() 
        // {
        //     if(Input.GetKeyDown(KeyCode.M))
        //     {
        //         BgmManager.Instance.Play(0);
        //     }
        //     else if(Input.GetKeyDown(KeyCode.N))
        //     {
        //         BgmManager.Instance.Pause();
        //     }

        //     _audio.GetSpectrumData(_spectrum, 0, _fftWindow);
        //     int i = 1;
        //     while (i < _spectrum.Length-1) 
        //     {
        //         Debug.DrawLine(new Vector3(i - 1, _spectrum[i] + 10, 0), new Vector3(i, _spectrum[i + 1] + 10, 0), Color.red);
        //         Debug.DrawLine(new Vector3(i - 1, Mathf.Log(_spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(_spectrum[i]) + 10, 2), Color.cyan);
        //         Debug.DrawLine(new Vector3(Mathf.Log(i - 1), _spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), _spectrum[i] - 10, 1), Color.green);
        //         Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(_spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(_spectrum[i]), 3), Color.yellow);
        //         i++;
        //     }
        // }
    }
}
