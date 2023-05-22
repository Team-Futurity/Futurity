using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
	public GaugeBarController gaugeBar;

	public int ComboCount { get { return comboCount; } }
	public int CurrentGauge { get { return currentGauge;} }

	private void Start()
	{
		currentGauge = 0;
		gaugeBar.SetGaugeFillAmount(0);
	}

	// ComboCount를 결정
	private void SetComboCount(bool isSucceed)
	{
		comboCount = isSucceed ? comboCount-1 : 0;

		if(comboCount == minComboCount - 1)
		{
			comboCount = maxComboCount;
		}
	}

	// SetComboCount()에서 결정된 comboCount를 기반으로 쌓일 게이지 연산 
	private int CalculateCurrentGauge(int hittedEnemyCount)
	{
		int addedGauge = -1;
		int compareComboCount = comboCount == 3 ? 0 : comboCount + 1;
		foreach(ComboCountData data in comboData)
		{
			if(data.ComboCount == compareComboCount) { addedGauge = data.AddedGauge; break; }
		}

		if(addedGauge == -1)
		{
			FDebug.LogError("[ComboGaugeSystem]Cannot Found ComboCountData");
			return -1;
		}

		return Mathf.CeilToInt(addedGauge + Mathf.Sqrt(addedGauge) * hittedEnemyCount);
	}

	public void SetComboGaugeProc(bool isSucceed, int hittedEnemyCount)
	{
		SetComboCount(isSucceed);

		if(!isSucceed || hittedEnemyCount == 0) { return; }

		int addedComboGauge = CalculateCurrentGauge(hittedEnemyCount);

		currentGauge = Mathf.Clamp(currentGauge + addedComboGauge, minComboGauge, maxComboGauge);

		gaugeBar.SetGaugeFillAmount((float)currentGauge / maxComboGauge);
	}
}
