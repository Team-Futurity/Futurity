using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitCountSystem))]
public class HitCountCustomEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var hitCountSystem = (HitCountSystem)target;

		// ���� ����
		//hitCountSystem.hitCountUI = hitCountSystem.hitCountUI == null ? GameObject.Find("ComboScore")?.GetComponent<NumberImageLoader>() : hitCountSystem.hitCountUI;
		//if(hitCountSystem.hitCountUI == null ) { FDebug.LogWarning("[ComboGaugeCustomEditor] ComboGaugeBar�� ã�� �� ����. Plyer ������Ʈ�� GaugeBar�� �Ҵ� �ʿ�."); }
	}
}
