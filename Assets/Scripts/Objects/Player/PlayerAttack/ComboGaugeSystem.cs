using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ComboCountData
{
	[field: SerializeField] public int ComboCount { get; private set; }
	[field: SerializeField] public int AddedGauge { get; private set; }

	public ComboCountData(int comboCount, int addedGauge = 0)
	{
		ComboCount = comboCount;
		AddedGauge = addedGauge;
	}
}

public class ComboGaugeSystem : MonoBehaviour
{
	public int minComboCount = 0;
	public int maxComboCount = 3;
	public int minComboGauge = 0;
	public int maxComboGauge = 100;
	private int comboCount;
	private int currentGauge;
	private int maxGauge;
	public List<ComboCountData> comboData;
	//public GaugeBarController gaugeBar;
	[HideInInspector] public UnityEvent<float> OnGaugeChanged;

	public int ComboCount { get { return comboCount; } }
	public int CurrentGauge { get { return currentGauge;} }

	private void Start()
	{
		comboCount = maxComboCount;
		currentGauge = 0;

		//if(gaugeBar == null) { FDebug.LogError("[ComboGaugeSystem] gaugeBar is Null. This sripct require GauegeBarController Component in Same Scene"); return; }

		//gaugeBar.SetGaugeFillAmount(currentGauge);
	}

	// ComboCount를 결정
	private void SetComboCount(bool isSucceed)
	{
		comboCount = isSucceed ? comboCount-1 : comboCount;

		if(comboCount == minComboCount - 1)
		{
			comboCount = maxComboCount;
		}
	}

	// SetComboCount()에서 결정된 comboCount를 기반으로 쌓일 게이지 연산 
	private int CalculateCurrentGauge(int hittedEnemyCount)
	{
		int addedGauge = -1;
		int compareComboCount = comboCount == maxComboCount ? minComboCount : comboCount;
		foreach(ComboCountData data in comboData)
		{
			if(data.ComboCount == compareComboCount) { addedGauge = data.AddedGauge; break; }
		}

		if(addedGauge == -1)
		{
			FDebug.LogError($"[ComboGaugeSystem]Cannot Found ComboCountData : {compareComboCount}");
			return 0;
		}

		return Mathf.CeilToInt(addedGauge + Mathf.Sqrt(addedGauge) * hittedEnemyCount);
	}

	public void SetComboGaugeProc(bool isSucceed, int hittedEnemyCount)
	{
		SetComboCount(isSucceed);

		if(!isSucceed || hittedEnemyCount == 0) { return; }

		int addedComboGauge = CalculateCurrentGauge(hittedEnemyCount);
		FDebug.Log("Combo : " + addedComboGauge);

		SetComboGauge(currentGauge + addedComboGauge);
	}

	private void SetComboGauge(int gauge)
	{
		currentGauge = Mathf.Clamp(gauge, minComboGauge, maxComboGauge);

		//if (gaugeBar != null)
		//{
		//	FDebug.Log($"gaugeBar.SetGaugeFillAmount((float)currentGauge / maxComboGauge) : {(float)currentGauge / maxComboGauge}");
		//	gaugeBar.SetGaugeFillAmount((float)currentGauge / maxComboGauge);
		//	OnGaugeChanged.Invoke(currentGauge);
		//}
	}

	public void ResetComboGauge()
	{
		SetComboGauge(minComboGauge);
	}

	public void ResetComboCount()
	{
		comboCount = maxComboCount;
	}
}
