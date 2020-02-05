using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using MIT.SamtleGame.Tools;
[RequireComponent(typeof(TMP_Text))]
public class DialogueBox : MonoBehaviour
{
	protected TMP_Text _textComponent;
	protected int _currentPage = 0;
	protected int _id;

	[Header("텍스트")]
	[Multiline(3)]
	public List<string> _textPages;

	[Tooltip("텍스트 출력 효과음")]
	public string _typingSoundName;
	[Tooltip("텍스트 출력 지연시간")]
	public float _delay = 0.33f;
    public bool _isTextChanged;
	public bool _isNextPage = false;
	public bool _isLoop = true;

	protected virtual void Initialization()
	{
		_textComponent = gameObject.GetComponent<TMP_Text>();
		_textComponent.text = _textPages[_currentPage];
	}

	private void Start() 
	{
		Initialization();
	}

	public virtual void Reset(int id, List<string> textPages, int page = 0, string sound = "")
    {
		_id = id;
		_currentPage = page;
		_typingSoundName = sound;
		_textPages = textPages;
		Initialization();
		StartCoroutine(RevealCharacters(_textComponent, _delay));
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
