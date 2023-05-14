using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour
{
	[SerializeField]
	private AudioManager audioManager;

	private void Start()
	{
		audioManager = AudioManager.instance;
	}

	public void SetMasterVolume(float volume)
	{
		FDebug.Log($"Sound : {volume}");
	}
}
