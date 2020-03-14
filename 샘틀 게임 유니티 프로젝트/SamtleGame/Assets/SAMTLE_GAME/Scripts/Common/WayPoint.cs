using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Stage2;
using MIT.SamtleGame.Stage2.NPC;

namespace MIT.SamtleGame.Tools
{
    public enum EventType { Talk, Fight, None, PassInfo, PlayerControll }

    public class WayPoint : Node
    {
        public EventType _type;
        public Direction _dir;
        public List<DialoguePage> _textPages;
        public int _id = -1;
        public void Init(int id)
        {
            _id = id;
        }
        public virtual Coroutine Trigger(MonoBehaviour mono)
        {
            switch(_type)
            {
                case EventType.Talk:
                    PassInfo();
                    return mono.StartCoroutine(Talk());
                case EventType.Fight:
                    return mono.StartCoroutine(Fight());
                case EventType.PassInfo:
                    PassInfo();
                    break;
                case EventType.PlayerControll:
                    BgmManager.Instance.Play(0);
                    PlayerControllerEvent.Trigger(true);
                    break;
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
                if(GameManager.Instance._npcs.ContainsKey(_id))
                {
                    Npc npc = GameManager.Instance._npcs[_id];
                    npc.Talk(false);
                }
                else
                {
                    Debug.Log("Key error");
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
            else
            {
                Debug.Log("key error");
            }
        }
    }
}
