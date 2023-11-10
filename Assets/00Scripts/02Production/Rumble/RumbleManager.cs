using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RumbleManager : Singleton<RumbleManager>
{
	private Gamepad pad;
	private bool isActive;
	private Coroutine rumbleCoroutine;

	public void RumblePulse(float lowFrequency, float highFrequency, float duration)
	{
		pad = Gamepad.current;
		isActive = InputActionManager.Instance.currentDevice is Gamepad;

		if(pad == null || !isActive) { return; }

		pad.SetMotorSpeeds(lowFrequency, highFrequency);

		if(rumbleCoroutine != null) { StopCoroutine(rumbleCoroutine); }

		rumbleCoroutine = StartCoroutine(StopRumble(duration, pad));
	}
	
	private IEnumerator StopRumble(float duration, Gamepad gamePad)
	{
		float elapsedTime = 0f;

		while(elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		gamePad.SetMotorSpeeds(0, 0);
	}
}
