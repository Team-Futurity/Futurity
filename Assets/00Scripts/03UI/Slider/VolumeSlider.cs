using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private BusType busType;

	private void Start()
	{
		if(volumeSlider == null)
		{
			volumeSlider = GetComponent<Slider>();
		}

		volumeSlider.onValueChanged.AddListener(OnSliderChanged);

		float currentVolume = AudioManager.Instance.GetVolume(busType);
		if (currentVolume < 0) { FDebug.LogWarning("This Bus is not Exist", GetType()); return; }

		volumeSlider.value = currentVolume;
	}

	public void OnSliderChanged(float value)
	{
		float ratio = value / volumeSlider.maxValue;

		float currentVolume = AudioManager.Instance.GetVolume(busType);
		if(currentVolume < 0) { FDebug.LogWarning("This Bus is not Exist", GetType()); return; }

		AudioManager.Instance.SetVolume(busType, ratio);
	}

	public float GetCurrentVolume()
	{
		return AudioManager.Instance.GetVolume(busType);
	}
}
