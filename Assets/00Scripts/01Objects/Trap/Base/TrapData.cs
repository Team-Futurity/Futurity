using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapData
{
	// Trap Code - Unique 
	// 함정 종류마다 가지고 있는 각자의 고유 코드
	// 추후 추가 가능성이 있다고 판단하여 선언만 진행함
	[field: SerializeField] public string TrapUID { get; private set; }
	
	
	// Trap Code - Not Unique
	// 함정 오브젝트의 고유 코드
	[field: SerializeField] public int TrapCode { get; private set; }

	// String
	// 함정 이름
	[field: SerializeField] public string TrapName { get; private set; }

	// Int
	// 함정의 발동 조건 ( 0 - NONE, 1 - 플레이어 접근, 2 - 플레이어 공격 )
	[field: SerializeField] public TrapCondition TrapCondition { get; private set; } 
	
	// 함정의 타입 ( 0 - NONE, 1 - FALL, 2 - Debuff, 3 - Explosion )
	[field: SerializeField] public TrapType TrapType { get; private set; }

	// 함정 효과 ( 0 - NONE, 1 - Stun, 3 - Damage )
	[field: SerializeField] public int TrapDebuff { get; private set; }

	// Float Data
	// 함정 피격 범위
	[field: SerializeField] public float TrapRange { get; private set; }
	
	// 함정 데미지
	[field: SerializeField] public float TrapDamage { get; private set; }
	
	// 함정 상태 이상 시간
	[field: SerializeField] public float TrapDuration { get; private set; }
	
	// 함정 쿨타임
	[field: SerializeField] public float TrapCooldowns { get; private set; }
}