using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NPC
{
    /*
    *   @desc 할배 NPC 클래스
    */
    public class GrandFather : MonoBehaviour
    {
        [SerializeField]
        [Header("NPC의 행동을 정의")]
        private Action _act;

        private void Start()
        {
            StartMove();
        }

        private void Move(Direction dir)
        {
            switch(dir)
            {
                case Direction.UP:
                    transform.transform.Translate(Vector2.up);
                    break;
                case Direction.DONW:
                    transform.transform.Translate(Vector2.down);
                    break;
                case Direction.RIGHT:
                    transform.transform.Translate(Vector2.right);
                    break;
                case Direction.LEFT:
                    transform.transform.Translate(Vector2.left);
                    break;
            }
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
