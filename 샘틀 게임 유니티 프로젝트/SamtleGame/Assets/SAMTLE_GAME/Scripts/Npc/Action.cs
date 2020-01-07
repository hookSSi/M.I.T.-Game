using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /*
    *   @desc NPC의 행동 목록들을 만들 수 있는 클래스
    */
    public enum Direction
    {
        UP,
        DONW,
        RIGHT,
        LEFT,
        NONE
    }

    [System.Serializable]
    public class Action
    {
        [Tooltip("이동 방향들을 정의")]
        public Direction[] moveDirs;
    }

}
