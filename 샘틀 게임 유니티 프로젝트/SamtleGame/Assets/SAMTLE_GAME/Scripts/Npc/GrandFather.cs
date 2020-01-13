using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MIT.SamtleGame.NPC
{
    /*
    *   @desc 할배 NPC 클래스
    */
    [RequireComponent(typeof(Animator))]
    public class GrandFather : Npc
    {
        [Header("NPC의 행동을 정의")]
        [SerializeField]
        private Action _act;
        [Header("현재 Npc 방향")]
        [SerializeField]
        private Vector2 _currentDir;
        private Animator _animator;

        protected override void Initialization() 
        {
            base.Initialization();
            _animator = GetComponent<Animator>();
            StartMove();
        }

        private void Move(Direction dir)
        {
            switch(dir)
            {
                case Direction.UP:
                    _animator.SetInteger("State", (int)NpcAnimState.Walk);
                    _currentDir = Vector2.up;
                    break;
                case Direction.DONW:
                    _animator.SetInteger("State", (int)NpcAnimState.Walk);
                    _currentDir = Vector2.down;
                    break;
                case Direction.RIGHT:
                    _animator.SetInteger("State", (int)NpcAnimState.Walk);
                    _currentDir = Vector2.right;
                    break;
                case Direction.LEFT:
                    _animator.SetInteger("State", (int)NpcAnimState.Walk);
                    _currentDir = Vector2.left;
                    break;
                case Direction.NONE:
                    _animator.SetInteger("State", (int)NpcAnimState.Idle);
                    break;
            }

            _animator.SetFloat("Horizontal", _currentDir.x);
            _animator.SetFloat("Vertical", _currentDir.y);

            transform.transform.Translate(_currentDir);
        }

        private void StartMove()
        {
            StartCoroutine("MoveCorountine");
        }

        IEnumerator MoveCorountine()
        {
            foreach(var dir in _act.moveDirs)
            {
                this.Move(dir);
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }
}
