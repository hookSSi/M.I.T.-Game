using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.NPC
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
    /*
    *   @desc NPC의 애니메이션 상태
    */
    public enum State
    {
        IDLE,
        WALK
    }


    [System.Serializable]
    public class Action
    {
        [Tooltip("이동 방향들을 정의")]
        public Direction[] moveDirs;
    }

}
