using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage3
{
    public class Chair : Interactive
    {
        private GameObject _character;
        private Animator _anim;

        public Player _player;
        public Transform _sitPos;
        public float _gap = 0.1f;

        protected override void Start()
        {
            base.Start();
            _character = _player._playerModel;
            _anim = _character.GetComponent<Animator>();
        }

        public override void Action()
		{
            if(_player._controller._isControllable)
            {
			    StartCoroutine(SitOnRoutine());
            }
		}

        public IEnumerator SitOnRoutine()
        {
            _player._controller._isControllable = false;

            while(!_player._controller._isControllable)
            {
                Vector3 targetDir;
                targetDir = new Vector3(_sitPos.position.x - _player.transform.position.x,
                                        0f,
                                        _sitPos.position.z - _player.transform.position.z);

                Quaternion rot = Quaternion.LookRotation(targetDir);
                _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, rot, 0.05f);

                Vector3 moveDir = Vector3.forward * 0.01f;
                _player.transform.Translate(moveDir);
                // 각도를 곱해서 구한다. 귀찮으니 일단은 대충 처리
                // _anim.SetFloat("Horizontal", moveDir * rot);
                // _anim.SetFloat("Vertical", moveDir * rot);
                _anim.SetFloat("Horizontal", 1f);

                if(Vector3.Distance(_player.transform.position, _sitPos.position) < _gap)
                {
                    _player.transform.rotation = this.transform.rotation;
                    _player._controller.SitOnChair();
                    _player._controller._isControllable = true;
                }

                yield return null;
            }
            yield break;
        }

        protected override void OnDrawGizmosSelected()
        {

        }
    }
}
