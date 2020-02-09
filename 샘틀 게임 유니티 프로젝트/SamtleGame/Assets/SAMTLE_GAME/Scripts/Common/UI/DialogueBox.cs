using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using MIT.SamtleGame.Tools;

/// string을 대체할 예정
[System.Serializable]
public struct DialoguePage
{
	[Multiline(3)]
	public string _text;
	[Range(0, 100)]
	public float _delayDuration; // 실행전 딜레이
	[Range(0, 100)]
	public float _duration; // 실행후 지속시간
}

[RequireComponent(typeof(TMP_Text))]
public class DialogueBox : MonoBehaviour
{
	protected TMP_Text _textComponent;
	protected int _currentPage = 0;
	protected int _id;
	protected float _delayDuration = 0.33f;
	protected float _duration = 0.33f;

	[Header("텍스트")]
	public List<DialoguePage> _textPages;

	[Tooltip("텍스트 출력 효과")]
	public float _typingDelay;
	public string _typingSoundName;
    public bool _isTextChanged;
	public bool _isNextPage = false;
	public bool _isLoop = true;
	public bool _isStartFirst = false;

	protected virtual void Initialization()
	{
		if(_textComponent == null)
			_textComponent = gameObject.GetComponent<TMP_Text>();
		_textComponent.text = _textPages[_currentPage]._text;
		_delayDuration = _textPages[_currentPage]._delayDuration;
		_duration = _textPages[_currentPage]._duration;
	}

	private void Start() 
	{
		Initialization();

		/// 스테이지1 보스 인트로 애니메이션 때문에 사용
		if(_isStartFirst)
			StartCoroutine(RevealCharacters(_textComponent));
	}

	public virtual void Reset(int id, List<DialoguePage> textPages, int page = 0, string sound = "")
    {
		_id = id;
		_currentPage = page;
		_typingSoundName = sound;
		_textPages = textPages;
		Initialization();
		StartCoroutine(RevealCharacters(_textComponent));
    }

	public virtual void NextPage()
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
			Debug.Log("페이지의 끝에 도달했습니다.");
		}

		_isNextPage = true;
	}

	protected virtual void PlaySound(char ch)
	{
		if(ch != ' ' && ch != '\n')
			SoundEvent.Trigger(_typingSoundName);
	}

	protected virtual IEnumerator RevealCharacters(TMP_Text textComponent)
	{
		textComponent.ForceMeshUpdate();

		TMP_TextInfo textInfo = textComponent.textInfo;

		int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
		int visibleCount = 0;
		textComponent.maxVisibleCharacters = visibleCount;

		yield return new WaitForSeconds(_delayDuration);
		while (true)
		{
			if (_isTextChanged)
			{
				totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
				_isTextChanged = false; 
			}

			if (_isNextPage)
			{
				yield return new WaitForSeconds(_duration);

				_isNextPage = false;
				visibleCount = 0;
				
				yield return new WaitForSeconds(_delayDuration);
			}

			if(0 < visibleCount && visibleCount < textComponent.text.Length)
			{
				char ch = textComponent.text[visibleCount - 1];
				PlaySound(ch);
			}

			textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
			
			yield return new WaitForSeconds(_typingDelay);
			visibleCount += 1;
		}
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
}
