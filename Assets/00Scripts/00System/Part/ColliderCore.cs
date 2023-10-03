using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCore : CoreAbility
{
	// 람다, 에타, 타우
	// 람다 : 시간 확인 후, 상태 이상 부여 -> Player
	// 에타 : 사망한 모든 몬스터의 자리에 공격 범위 생성, 상태 이상 부여 -> Death Monster

	public ColliderCoreType coreActiveType;

	// 람다 & 에타
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
