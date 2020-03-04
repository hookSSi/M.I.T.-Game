using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage3
{
    public class LaunchPad : Interactive
    {
        [Header("런치패드 정보"), Space(20)]
        public MusicAnalyzer _musicAnalyzer;
        public List<Pad> pads = new List<Pad>();
        public Tweens.TweenCurve _curve;

        public override void Action()
        {
            GameManager.Instance._player._controller.SetMovable(false);
            GameManager.Instance._player._controller.FocusIn();

            BgmManager.Instance.Play(0, true);

            GameManager.Instance.Musician();
        }
    }
}
