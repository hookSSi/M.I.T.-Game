using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using MIT.SamtleGame.Tools;
using MIT.SamtleGame.Attributes;

/// string을 대체할 예정
[System.Serializable]
public struct DialoguePage
{
	[Multiline(3)]
	public string _text;
	[Range(0, 100)]
	public float _delay; // 실행전 딜레이
	[Range(0, 100)]
	public float _duration; // 실행후 지속시간

	public static DialoguePage CreatePage(string text, float delay = 0.1f, float duration = 0.1f)
	{
		DialoguePage page = new DialoguePage();
		page._text = text;
		page._delay = delay;
		page._duration = duration;

		return page;
	}
}

[RequireComponent(typeof(TMP_Text))]
[SelectionBase]
public class DialogueBox : MonoBehaviour
{
	protected TMP_Text _textComponent;
	[SerializeField]
	protected int _currentPage = 0;
	protected float _delayDuration = 0.33f;
	protected float _duration = 0.33f;
	protected int _id;
	protected Coroutine _currentRoutine = null; // 현재 재생중인 코루틴

	[Header("텍스트")]
	public List<DialoguePage> _textPages;

	[Header("텍스트 출력 효과")]
	public float _typingDelay;
	[GameAudio] public string _typingSfx;
    public bool _isTextChanged;
	public bool _isNextPage = false;
	public bool _isLoop = true;
	public bool _isStartFirst = false;

	protected virtual void Initialization()
	{
		if(_textComponent == null)
			_textComponent = gameObject.GetComponent<TMP_Text>();

		_textComponent.text = _textPages[_currentPage]._text;
		_delayDuration = _textPages[_currentPage]._delay;
		_duration = _textPages[_currentPage]._duration;
	}

	private void Start() 
	{
		Initialization();

		/// 스테이지1 보스 인트로 애니메이션 때문에 사용
		if(_isStartFirst)
			StartCoroutine(RevealCharacters(_textComponent));
		else
		{
			_textComponent.maxVisibleCharacters = 0;
		}
	}

	public virtual void Reset(int id, List<DialoguePage> textPages, int page = 0, string sound = "")
    {
		_id = id;
		_currentPage = page;
		_typingSfx = sound;
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
			SoundEvent.Trigger(_typingSfx);
	}

	public void PlayText()
	{
		if(_currentRoutine != null)
			StopCoroutine(_currentRoutine);
		_currentRoutine = StartCoroutine(RevealCharacters(_textComponent));
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
