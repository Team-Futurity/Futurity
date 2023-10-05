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

		attackCore.attackType = (AttackCoreType)EditorGUILayout.EnumPopup("���� Ÿ��", attackCore.attackType);
		
		EditorGUILayout.Space(10);
		EditorGUILayout.LabelField("��ǰ ȿ�� ����");
		EditorGUILayout.Space(10);

		switch (attackCore.attackType)
		{
			case AttackCoreType.ADD_DAMAGE:
				attackCore.isStateTransition = EditorGUILayout.Toggle("���� ȿ��", attackCore.isStateTransition);
				EditorGUILayout.Space(20);

				if (!attackCore.isStateTransition)
				{
					attackCore.attackDamage = EditorGUILayout.FloatField("���� ������", attackCore.attackDamage);
					attackCore.colliderRadius = EditorGUILayout.FloatField("���� �ݶ��̴� ����", attackCore.colliderRadius);
				}
				break;
			case AttackCoreType.ADD_ODD_STATE:
				EditorGUILayout.LabelField("���� �̻��� BuffGiver�� ���ؼ� ó���ٶ�");
				break;
		}
		
		

		if (attackCore.isStateTransition)
		{
			attackCore.transitionColliderRadius =
				EditorGUILayout.FloatField("���� �ݶ��̴� ����", attackCore.transitionColliderRadius);
			attackCore.transitionDamage = EditorGUILayout.FloatField("���� ������", attackCore.transitionDamage);
			attackCore.transitionCount = EditorGUILayout.IntField("���� Ƚ��", attackCore.transitionCount);
			attackCore.colliderRadius = attackCore.transitionColliderRadius;
		}
	}
}
