using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage3
{
    [RequireComponent(typeof(MusicAnalyzer))]
    public class LaunchPad : Interactive
    {
        [Header("런치패드 정보"), Space(20)]
        public MusicAnalyzer _musicAnalyzer;
        public List<Pad> _pads = new List<Pad>();
        public Tweens.TweenCurve _curve;
        public float  _threshold;

        protected override void Start()
        {
            base.Start();
            _musicAnalyzer = GetComponent<MusicAnalyzer>();
            this.gameObject.GetComponentsInChildren(_pads);
        }

        public override void Action()
        {
            GameManager.Instance._player._controller.SetMovable(false);
            GameManager.Instance._player._controller.FocusIn();

            PlayLaunchPad();
            GameManager.Instance.Musician();
        }

        private void PlayLaunchPad()
        {
            StartCoroutine(PadLightLoop());
        }

        IEnumerator PadLightLoop()
        {
            BgmManager.Instance.Play(0, true);
            _musicAnalyzer.SetMusic(BgmManager.Instance.GetAudio());

            while(BgmManager.Instance.IsPlaying())
            {
                _musicAnalyzer.AnalyzeMusic();
                float[] spectrum = _musicAnalyzer.GetSpectrum();
                bool[] checks = new bool[_pads.Count];

                for(int i = 0; i < spectrum.Length; i++)
                {
                    if(spectrum[i] > _threshold)
                    {
                        checks[i % _pads.Count] = true;
                    }
                }

                for(int i = 0; i < checks.Length; i++)
                {
                    if(checks[i])
                        _pads[i].Press();
                }

                yield return null;
            }

            yield break;
        }
    }
}
