using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCore : CoreAbility
{
	// ����, �ο�, �۽Ƿ�, 
	// ���� : �÷��̾� ���� ��, ���� ����
	// �ο� : �÷��̾� ���� ��, ���� ������ ����
	// �۽Ƿ� : �÷��̾� ���� ��, �ִ� 6�� ���̵Ǵ� ������ ����
	
	
	// Attack Core Type
	// 1. ���� �̻� �ο�
	// 2. ���� ������
	[SerializeField]
	private AttackCoreType attackType;
	
	// ���� ��, ���� ȿ��
	[SerializeField]
	private bool isStateTransition;

	private float attackDamage = .0f;
	
	// ���� ȿ���� ������ ���, �浹 Circle ������
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	
	// Monster Data
	
	protected override void OnPartAbility(EnemyController enemy)
	{
		// Attack ��, Buff ����
	}
	
}
