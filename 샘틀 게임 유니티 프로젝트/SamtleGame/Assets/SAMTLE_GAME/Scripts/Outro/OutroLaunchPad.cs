using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIT.SamtleGame.Stage3;

public class OutroLaunchPad : MonoBehaviour
{
	public MusicAnalyzer _musicAnalyzer;
	public List<Pad> _pads = new List<Pad>();
	public AudioSource audio;
	public float _threshold;
	// Start is called before the first frame update
	void Start()
	{
		_musicAnalyzer = GetComponent<MusicAnalyzer>();
		this.gameObject.GetComponentsInChildren(_pads);
		PlayLaunchPad();
	}

	private void PlayLaunchPad()
	{
		StartCoroutine(PadLightLoop());
	}

	IEnumerator PadLightLoop()
	{
		_musicAnalyzer.SetMusic(audio);

		while (true)
		{
			_musicAnalyzer.AnalyzeMusic();
			float[] spectrum = _musicAnalyzer.GetSpectrum();
			bool[] checks = new bool[_pads.Count];

			for (int i = 0; i < spectrum.Length; i++)
			{
				if (spectrum[i] > _threshold)
				{
					checks[i % _pads.Count] = true;
				}
			}

			for (int i = 0; i < checks.Length; i++)
			{
				if (checks[i])
					_pads[i].Press();
			}

			yield return null;
		}

		yield break;
	}
}
