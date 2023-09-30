using Cinemachine;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraFollowTarget : MonoBehaviour
{
	private CinemachineBrain cameraBrain;
	private IEnumerator targetShake;

	private void Start()
	{
		cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
	}

	public void StartTargetShake(float duration, float magnitude)
	{
		targetShake = TargetShake(duration, magnitude);
		StartCoroutine(targetShake);
	}
	
	private IEnumerator TargetShake(float duration, float magnitude)
	{
		cameraBrain.m_IgnoreTimeScale = true;
		
		Vector3 originPos = Vector3.zero;
		float elapsed = 0.0f;

		while (elapsed < duration)
		{
			float x = Random.Range(-1, 1) * magnitude;
			float y = Random.Range(-1, 1) * magnitude;
			float z = Random.Range(-1, 1) * magnitude;
			
			transform.localPosition = new Vector3(x, y, z);

			elapsed += Time.unscaledDeltaTime;
			yield return null;
		}

		transform.localPosition = originPos;
		cameraBrain.m_IgnoreTimeScale = false;
	}
}
