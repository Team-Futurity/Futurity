using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundSliderButton : UIButton
{
	public VolumeSlider volumeSlider;

	private float soundRatio = .0f;

	private void Start()
	{
		soundRatio = volumeSlider.GetCurrentVolume();
	}
	protected override void ActiveFunc()
	{
		FDebug.Log(" Slider는 별도의 효과가 존재하지 않습니다. ");
	}

	protected override void OnLeftActive()
	{
		soundRatio -= 0.1f;
		if (soundRatio < 0f) soundRatio = 0f;

		volumeSlider.OnSliderChanged(soundRatio);
	}

	protected override void OnRightActive()
	{
		soundRatio += 0.1f;
		if (soundRatio > 1f) soundRatio = 1f;

		volumeSlider.OnSliderChanged(soundRatio);
	}
}
