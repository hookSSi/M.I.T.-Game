using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using MIT.SamtleGame.Tools;

public class IntroDialogueBox : DialogueBox
{
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

			if (visibleCount > totalVisibleCharacters)
			{
				FadeInEvent.Trigger(1);
				
				NextPage();
                visibleCount = 0;

				ChangeImageEvent.Trigger(_currentPage);
				FadeOutEvent.Trigger(1);
				yield return null;
			}

            PlaySound();
			textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
			
            yield return new WaitForSeconds(delay);

			visibleCount += 1;
		}
	}
}
