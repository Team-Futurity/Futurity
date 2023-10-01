using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCore : CoreAbility
{
	// 람다, 에타, 타우
	// 람다 : 시간 확인 후, 상태 이상 부여 -> Player
	// 에타 : 사망한 모든 몬스터의 자리에 공격 범위 생성, 상태 이상 부여 -> Death Monster

	// 람다 & 에타
	public float colliderRadius = .0f;
	public float colliderExecuteCycle = .0f;
	
	protected override void OnPartAbility(Enemy enemy)
	{
		
	}

	private void ExploreEnemy()
	{
		
	}
}
