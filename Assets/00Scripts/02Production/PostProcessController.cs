using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
	// private Volume volume;
	// private Vignette vignette;
	// private HealthWarningEffect healthEffect;
 //
	// private bool isHit;
	// private float curTime;
	// private float vignetteTime;
 //
	// private void Start()
 //    {
 //        volume = GetComponent<Volume>();
 //        volume.profile.TryGet(out vignette);
 //
 //        healthEffect = Camera.main.GetComponent<HealthWarningEffect>();
 //    }
 //
	// private void Update()
	// {
	// 	if (isHit && healthEffect.IsEffectEnable == false)
	// 	{
	// 		if (vignetteTime > curTime)
	// 		{
	// 			vignette.active = true;
	// 			curTime += Time.deltaTime;
	// 			vignette.intensity.value = 0 + curTime;
	// 		}
	// 		else
	// 		{
	// 			vignette.intensity.value = 0;
	// 			curTime= 0;
	// 			isHit = false;
	// 		}
	// 	}
	//
	// }
 //
	// public void SetVignette(float time)
	// {
	// 	vignetteTime = time;
	// 	isHit = true;
	// }
}
