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

		// 변수 변경
		//hitCountSystem.hitCountUI = hitCountSystem.hitCountUI == null ? GameObject.Find("ComboScore")?.GetComponent<NumberImageLoader>() : hitCountSystem.hitCountUI;
		//if(hitCountSystem.hitCountUI == null ) { FDebug.LogWarning("[ComboGaugeCustomEditor] ComboGaugeBar를 찾을 수 없음. Plyer 오브젝트에 GaugeBar에 할당 필요."); }
	}
}
