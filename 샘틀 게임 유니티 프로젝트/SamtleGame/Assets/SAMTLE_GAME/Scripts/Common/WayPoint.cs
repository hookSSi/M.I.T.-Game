using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2;
using MIT.SamtleGame.Stage2.NPC;

namespace MIT.SamtleGame.Tools
{
    public enum EventType { Talk, Fight, None }

    public class WayPoint : Node
    {
        public EventType _type;
        public Direction _dir;
        public List<DialoguePage> _textPages;
        public int _id = -1;

        public virtual Coroutine Trigger(MonoBehaviour mono)
        {
            PassInfo();
            switch(_type)
            {
                case EventType.Talk:
                    return mono.StartCoroutine(Talk());
                case EventType.Fight:
                    return mono.StartCoroutine(Fight());
                case EventType.None:
                    break;
            }

            return null;
        }

        protected virtual IEnumerator Talk()
        {
            /// id가 존재하는 걸로 Player가 접근하려고 하면 버그일어남
            if(DialogueManager.Instance._isEnd)
            {
                Debug.Log("대화");
                if(GameManager.Instance._npcs.ContainsKey(_id))
                {
                    Npc npc = GameManager.Instance._npcs[_id];
                    npc.Talk(false);
                }
                else
                {
                    Debug.Log("키가 없습니다.");
                }
            }

            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(Wait());
        }

        protected virtual IEnumerator Fight()
        {
            Debug.Log("전투");
            yield return new WaitForSeconds(1f);
        }

        protected virtual IEnumerator Wait()
        {
            while(!DialogueManager.Instance._isEnd)
            {
                yield return null;
            }

            yield break;
        }

        protected virtual void PassInfo()
        {
            SetNpcTextPages();
        }

        protected void SetNpcTextPages()
        {
            if(GameManager.Instance._npcs.ContainsKey(_id))
            {
                Npc npc = GameManager.Instance._npcs[_id];
                npc.ChangeTextPage(_textPages);
            }
        }
    }
}
