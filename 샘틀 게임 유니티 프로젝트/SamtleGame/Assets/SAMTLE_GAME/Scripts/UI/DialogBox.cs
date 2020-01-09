using UnityEngine;
using System.Collections;
using TMPro;

public class DialogBox : MonoBehaviour
{
	private TMP_Text _textComponent;
	[Tooltip("텍스트 출력 효과음")]
	public AudioClip _typingSound;
	[Tooltip("텍스트 출력 지연시간")]
	[SerializeField]
	private float _delay;
    public bool _isTextChanged;

	private void Awake ()
	{
		_textComponent = gameObject.GetComponent<TMP_Text>();
	}

	private void Start() 
	{
		StartCoroutine(RevealCharacters(_textComponent, _delay));
	}

	private void OnEnable() 
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
	}

	private void OnDisable() 
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
	}

	void ON_TEXT_CHANGED(Object obj)
	{
		_isTextChanged = true;
	}

	void PlaySound()
	{
		
	}

	/// <summary>
	/// Method revealing the text one character at a time.
	/// </summary>
	/// <returns></returns>
	IEnumerator RevealCharacters(TMP_Text textComponent, float delay)
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
				yield break;
			}

			PlaySound();
			textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
			yield return new WaitForSeconds(delay);
			
			visibleCount += 1;
		}
	}
}
