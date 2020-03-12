using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

namespace MIT.SamtleGame.Stage2.NPC
{
    /*
    *   @desc 회장 클래스
    */
    public class ChairMan : EventNpc
    {
        protected override void Update()
        {
            base.Update();

            _animator.SetFloat("Horizontal", _currentDir.x);
            _animator.SetFloat("Vertical", _currentDir.y);
        }

        protected override IEnumerator EventRoutine()
        {
            /// 플레이어에게 이동
            yield return StartCoroutine(MoveToPlayerRoutine());

            Talk(false);
            yield return WaitUntillTalkEnd();

            //PokemonBattle("C++", "동방컴");
            //yield return WaitUntillBattleEnd();

            List<DialoguePage> textPages = new List<DialoguePage>();
            textPages.Add(DialoguePage.CreatePage("코딩하느라 고생했어, 와! 정말 잘 만들었는걸"));
            textPages.Add(DialoguePage.CreatePage("잠시 의자에 앉아서 기다려줄래? 지원서를 찾아야해서"));
            textPages.Add(DialoguePage.CreatePage("뭐? 관심없다고? 에이 팅기지말고 기다려봐"));
            _textPages = textPages;
            Talk(false);
            yield return WaitUntillTalkEnd();
            
            yield return new WaitForSeconds(1f);
            LoadingSceneManager.LoadScene("동방");
            yield break;
        }

        void PokemonSelector()
        {

        }
    }
}
