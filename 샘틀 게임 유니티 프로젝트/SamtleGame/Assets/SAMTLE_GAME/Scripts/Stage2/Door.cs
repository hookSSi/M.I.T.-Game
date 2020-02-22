using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;
using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage2
{    
    [SelectionBase]
    public class Door : MonoBehaviour
    {
        [Header("정보")]
        public Transform _dest; // 목적지
        public Direction _dir = Direction.NONE; // 목적지에서 대상의 바라보는 방향
        public bool _isActive = true;
        public string _sound = "";
        public Tag _tag;
        
        [Header("FadeInOut"), Space(20)]
        public float _fadeInTime = 0.1f;
        public Tweens.TweenCurve _fadeInTween;
        public float _fadeOutTime = 0.1f;
        public Tweens.TweenCurve _fadeOutTween;

        private void Update() 
        {
            CheckColEntered();
        }

        protected virtual void CheckColEntered()
        {
            if(_isActive)
            {
                List<Collider2D> cols = ColliderChecker.GetColliders(this.transform.position, this.transform.position, _tag.GetTag());

                foreach(var col in cols)
                {
                    // 진입
                    FadeInEvent.Trigger(_fadeInTime, _fadeInTween);
                    SoundEvent.Trigger(_sound);

                    // 진입후
                    col.transform.position = _dest.transform.position;
                    col.GetComponent<PlayerController>().SetDirection(_dir);
                    FadeOutEvent.Trigger(_fadeOutTime, _fadeOutTween);
                }

                _isActive = false;
            }
        }

        protected void OnDrawGizmosSelected()
        {
            if(_dest == null)
            {
                // 출구 생성
                GameObject obj = new GameObject();
                obj.name = "출구";
                obj.transform.position = this.transform.position;
                _dest = obj.transform;
                obj.transform.SetParent(this.transform);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_dest.position, Vector3.one / 2);
            }
        }
    }
}
