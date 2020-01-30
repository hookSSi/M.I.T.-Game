﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using MIT.SamtleGame.Tools;
[RequireComponent(typeof(TMP_Text))]
public class DialogueBox : MonoBehaviour
{
	protected TMP_Text _textComponent;
	protected int _currentPage = 0;

	[Header("텍스트")]
	[Multiline(3)]
	public List<string> _textPages;

	[Tooltip("텍스트 출력 효과음")]
	public string _typingSoundName;
	[Tooltip("텍스트 출력 지연시간")]
	public float _delay;
    public bool _isTextChanged;
	public bool _isNextPage = false;

	protected virtual void Initialization()
	{
		_textComponent = gameObject.GetComponent<TMP_Text>();
		_textComponent.text = _textPages[_currentPage];
	}

	private void Start() 
	{
		Initialization();
		StartCoroutine(RevealCharacters(_textComponent, _delay));
	}

	protected virtual void OnEnable() 
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
	}

	protected virtual void OnDisable() 
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
	}

	protected void ON_TEXT_CHANGED(Object obj)
	{
		_isTextChanged = true;
	}

	public virtual void NextPage()
	{
		if(_currentPage < _textPages.Count - 1)
		{
			_currentPage++;
			_textComponent.maxVisibleCharacters = 0;
			_textComponent.text = _textPages[_currentPage];
		}
		else
		{
			Debug.Log("페이지의 끝에 도달했습니다.");
		}

		_isNextPage = true;
	}

	protected virtual void PlaySound(char ch)
	{
		
		if(ch != ' ' && ch != '\n')
			SoundEvent.Trigger(_typingSoundName);
	}

	/// <summary>
	/// Method revealing the text one character at a time.
	/// </summary>
	/// <returns></returns>
	protected virtual IEnumerator RevealCharacters(TMP_Text textComponent, float delay)
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

			if (_isNextPage)
			{
				_isNextPage = false;
				visibleCount = 0;
				
				yield return null;
			}

			if(0 < visibleCount && visibleCount < textComponent.text.Length)
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
