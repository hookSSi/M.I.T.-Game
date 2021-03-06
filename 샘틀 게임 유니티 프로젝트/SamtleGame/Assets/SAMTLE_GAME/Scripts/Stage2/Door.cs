﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2.Tools;
using MIT.SamtleGame.Stage2.NPC;
using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Attributes;

namespace MIT.SamtleGame.Stage2
{    
    [SelectionBase]
    public class Door : MonoBehaviour
    {
        [Header("정보")]
        public Transform _dest; // 목적지
        public Direction _dir = Direction.NONE; // 목적지에서 대상의 바라보는 방향
        public bool _isActive = true;
        [GameAudio] public string _sound;
        [GameBgm] public string _newTrack;
        public Tag[] _tag;
        
        [Header("FadeInOut"), Space(20)]
        public float _fadeInTime = 0.33f;
        public Tweens.TweenCurve _fadeInTween;
        public float _fadeOutTime = 0.33f;
        public Tweens.TweenCurve _fadeOutTween;

        private void Update() 
        {
            CheckColEntered();
        }

        protected virtual void CheckColEntered()
        {
            if(_isActive)
            {
                List<Collider2D> cols = ColliderChecker.GetColliders(this.transform.position, this.transform.position, _tag);

                foreach(var col in cols)
                {
                    if(col.tag == "Npc")
                    {
                        col.GetComponent<EventNpc>().Delete();
                    }
                    if(col.tag == "Player")
                    {
                        PlayerController player = col.GetComponent<PlayerController>();
                        StartCoroutine(TeleportPlayer(player));
                    }
                }
            }
        }

        IEnumerator TeleportPlayer(PlayerController player)
        {
            // 플레이어가 다 움직일때 까지 기다림
            yield return new WaitWhile(() => player._isMoving);

            // 진입
            FadeInEvent.Trigger(_fadeInTime, _fadeInTween);
            SoundEvent.Trigger(_sound);

            // 진입후
            player.transform.position = _dest.transform.position;
            player.SetDirection(_dir);
            FadeOutEvent.Trigger(_fadeOutTime, _fadeOutTween);

            yield return new WaitForSeconds(_fadeInTime);

            BgmManager.Instance.Play(_newTrack);

            yield break;
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
