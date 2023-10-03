using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCore : CoreAbility
{
	// ����, ��Ÿ, Ÿ��
	// ���� : �ð� Ȯ�� ��, ���� �̻� �ο� -> Player
	// ��Ÿ : ����� ��� ������ �ڸ��� ���� ���� ����, ���� �̻� �ο� -> Death Monster

	public ColliderCoreType coreActiveType;

	// ���� & ��Ÿ
	public float colliderRadius = .0f;
	public float colliderExecuteCycle = .0f;

	private float timer = .0f;
	
	protected override void OnPartAbility(Enemy enemy)
	{
		switch (coreActiveType)
		{
			case ColliderCoreType.ON_SCHEDULE_TIME:
				break;
			
			case ColliderCoreType.ENEMY_DEATH:
				break;
		}
	}

	private void Update()
	{
		if (coreActiveType != ColliderCoreType.ON_SCHEDULE_TIME)
		{
			return;
		}

		timer += Time.deltaTime;

		if (timer >= colliderExecuteCycle)
		{
			timer = .0f;
			
			ExploreEnemy();
		}
	}

	private void ExploreEnemy()
	{
		
	}
}
