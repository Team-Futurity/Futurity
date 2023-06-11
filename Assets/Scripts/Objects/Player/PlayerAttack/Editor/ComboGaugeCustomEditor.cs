using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComboGaugeSystem))]
public class ComboGaugeCustomEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var comboGaugeSystem = (ComboGaugeSystem)target;

		// 변수 변경
		comboGaugeSystem.gaugeBar = comboGaugeSystem.gaugeBar == null ? GameObject.Find("HitGauge")?.GetComponent<GaugeBarController>() : comboGaugeSystem.gaugeBar;
		//if(comboGaugeSystem.gaugeBar == null ) { FDebug.LogWarning("[ComboGaugeCustomEditor] ComboGaugeBar를 찾을 수 없음. Plyer 오브젝트에 GaugeBar에 할당 필요."); }

		// ComboData 리스트 변경
		comboGaugeSystem.comboData = comboGaugeSystem.comboData == null ? new List<ComboCountData>() : comboGaugeSystem.comboData;
		var list = comboGaugeSystem.comboData;

		int minCount = comboGaugeSystem.minComboCount;
		int maxCount = comboGaugeSystem.maxComboCount;
		int comboCount = maxCount - minCount;
		
		if (list.Count == comboCount) { return; }

		if (minCount < maxCount)
		{
			if (list.Count > comboCount)
			{
				int removeValue = list.Count - comboCount;
				for (int removeCount = 0; removeCount < removeValue; removeCount++)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
			else
			{
				int addValue = comboCount - list.Count;
				for (int removeCount = 0; removeCount < addValue; removeCount++)
				{
					list.Add(new ComboCountData(minCount + list.Count));
				}
			}
		}
		else
		{
			list.Clear();
		}
	}
}
