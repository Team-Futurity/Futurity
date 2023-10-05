using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackCore))]
public class AttackCoreCustomEditor : Editor
{
	private AttackCore attackCore;

	private void OnEnable()
	{
		attackCore = (AttackCore)target;
	}

	public override void OnInspectorGUI()
	{
		if (attackCore == null)
		{
			return;
		}

		attackCore.attackType = (AttackCoreType)EditorGUILayout.EnumPopup("공격 타입", attackCore.attackType);
		
		EditorGUILayout.Space(10);
		EditorGUILayout.LabelField("부품 효과 설정");
		EditorGUILayout.Space(10);

		switch (attackCore.attackType)
		{
			case AttackCoreType.ADD_DAMAGE:
				attackCore.isStateTransition = EditorGUILayout.Toggle("전이 효과", attackCore.isStateTransition);
				EditorGUILayout.Space(20);

				if (!attackCore.isStateTransition)
				{
					attackCore.attackDamage = EditorGUILayout.FloatField("피해 데미지", attackCore.attackDamage);
					attackCore.colliderRadius = EditorGUILayout.FloatField("공격 콜라이더 범위", attackCore.colliderRadius);
				}
				break;
			case AttackCoreType.ADD_ODD_STATE:
				EditorGUILayout.LabelField("상태 이상은 BuffGiver를 통해서 처리바람");
				break;
		}
		
		

		if (attackCore.isStateTransition)
		{
			attackCore.transitionColliderRadius =
				EditorGUILayout.FloatField("전이 콜라이더 범위", attackCore.transitionColliderRadius);
			attackCore.transitionDamage = EditorGUILayout.FloatField("전이 데미지", attackCore.transitionDamage);
			attackCore.transitionCount = EditorGUILayout.IntField("전이 횟수", attackCore.transitionCount);
			attackCore.colliderRadius = attackCore.transitionColliderRadius;
		}
	}
}
