using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
	[Header("공격 슬로우모션 데이터")] 
	[SerializeField] private List<AttackSlowMotionData> slowMotionData;
	private IEnumerator attackSlowMotion;

	public void StartAttackSlowMotion(int index)
	{
		attackSlowMotion = AttackSlowMotion(index);
		StartCoroutine(attackSlowMotion);
	}
	
	private IEnumerator AttackSlowMotion(int index)
	{
		Time.timeScale = slowMotionData[index].MinTimeScale;

		yield return new WaitForSecondsRealtime(slowMotionData[index].DurationMin);

		Time.timeScale = slowMotionData[index].MaxTimeScale;

		yield return new WaitForSecondsRealtime(slowMotionData[index].DurationMax);

		Time.timeScale = 1.0f;
	}
}
