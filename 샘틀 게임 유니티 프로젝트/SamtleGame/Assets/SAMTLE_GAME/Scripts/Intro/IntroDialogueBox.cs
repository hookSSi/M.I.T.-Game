using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using MIT.SamtleGame.Tools;

public class IntroDialogueBox : DialogueBox
{
	private bool _isPageEnd = false;

	public override void NextPage()
	{
		if(_currentPage < _textPages.Count - 1)
		{
			_currentPage++;
			_textComponent.maxVisibleCharacters = 0;
			_textComponent.text = _textPages[_currentPage];
		}
		else
		{
			IntroEvent.Trigger();
			_isPageEnd = true;
			Debug.Log("페이지의 끝에 도달했습니다.");
		}

		_isNextPage = true;
	}

	protected override IEnumerator RevealCharacters(TMP_Text textComponent, float delay)
	{
		textComponent.ForceMeshUpdate();

		TMP_TextInfo textInfo = textComponent.textInfo;

		int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
		int visibleCount = 0;

		while (true)
		{
			if (_isTextChanged)
			{
				totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
				_isTextChanged = false; 
			}

			if ( visibleCount > totalVisibleCharacters )
			{
				yield return new WaitForSeconds(1);
				FadeInEvent.Trigger(1);
				
				NextPage();

				if(_isPageEnd)
				{
					FadeOutEvent.Trigger(0);
					yield break;
				}
					
                visibleCount = 0;

				yield return new WaitForSeconds(1);
				ChangeImageEvent.Trigger(_currentPage);
				FadeOutEvent.Trigger(1);
				yield return null;
			}

			if( 0 < visibleCount )
			{
				char ch = textComponent.text[visibleCount - 1];
				PlaySound(ch);
			}

			textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            yield return new WaitForSeconds(delay);

			visibleCount += 1;
		}
	}
}
