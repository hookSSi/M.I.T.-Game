using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace MIT.SamtleGame.Stage2
{
    public class BattleDialogueBox : DialogueBox
    {
        protected override void Initialization()
        {
            base.Initialization();
            DialogueManager.Instance._isEnd = false;
        }

        public override void NextPage()
        {
            _currentPage++;

            if(_currentPage < _textPages.Count)
            {
                _textComponent.maxVisibleCharacters = 0;
                _isNextPage = true;
                base.Initialization();
            }
            else
            {
                Debug.Log("페이지의 끝에 도달했습니다.");
                DialogueManager.Instance._isEnd = true;
            }
        }

        public virtual IEnumerator WaitUntilInput(float delay = 1f)
        {
            /// 지정한 시간만큼 기다리고 자동으로 넘김
            yield return new WaitForSeconds(delay);

            Debug.LogFormat("{0} 다음 페이지", _currentPage);
            NextPage();
            yield break;
        }

        protected override IEnumerator RevealCharacters(TMP_Text textComponent)
        {
            textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = textComponent.textInfo;

            int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;
            textComponent.maxVisibleCharacters = visibleCount;

            yield return new WaitForSeconds(_delayDuration);
            while (!DialogueManager.Instance._isEnd)
            {
                if (_isTextChanged)
                {
                    totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                    _isTextChanged = false; 
                }

                /// 다음 페이지 경우 관련 변수 초기화
                if (_isNextPage)
                {
                    yield return new WaitForSeconds(_duration);

                    _isNextPage = false;
                    visibleCount = 0;
                    
                    yield return new WaitForSeconds(_delayDuration);
                }

                /// 사운드 처리
                if(0 < visibleCount && visibleCount < textComponent.text.Length)
                {
                    char ch = textComponent.text[visibleCount - 1];
                    PlaySound(ch);
                }

                /// 한 글자씩 밝혀짐
                textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
                yield return new WaitForSeconds(_typingDelay);
                visibleCount += 1;

                /// 다음 페이지를 기다림
                if(visibleCount > textComponent.text.Length)
                {
                    Debug.LogFormat("{0} 페이지 대화가 끝났습니다.", _currentPage);

                    yield return StartCoroutine(WaitUntilInput());
                    yield return new WaitForSeconds(0.33f);
                }
            }

            /// 대화 끝
            DialogueEvent.Trigger(_id, null, 0, "", DialogueStatus.End);
            yield return new WaitForSeconds(1f);
            yield break;
        }
    }
}
