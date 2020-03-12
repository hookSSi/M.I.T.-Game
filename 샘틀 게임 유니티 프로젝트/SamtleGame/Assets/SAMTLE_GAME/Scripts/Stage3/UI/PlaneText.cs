using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneText : DialogueBox
{
	public override void NextPage()
	{
		if(_currentPage < _textPages.Count - 1)
		{
			_currentPage++;
			_textComponent.maxVisibleCharacters = 0;
			Initialization();
		}
		else
		{
			if(!_isLoop)
				this.gameObject.SetActive(false);
			else
            {
                _currentPage = 0;
                _textComponent.maxVisibleCharacters = 0;
			    Initialization();
            }
		}

		_isNextPage = true;
	}
}
