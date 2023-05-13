using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapData
{
	// Trap Code - Not Unique
	// 함정 오브젝트의 고유 코드
	public int code;

	// Trap Code - Unique 
	// 함정 종류마다 가지고 있는 각자의 고유 코드
	public string uID;

	// String
	// 함정 이름
	public string name;

	// Int
	// 함정의 발동 조건 ( 0 - NONE, 1 - 플레이어 접근, 2 - 플레이어 공격 )
	public int condition;
	// 함정의 타입 ( 0 - NONE, 1 - FALL, 2 - Debuff, 3 - Explosion )
	public int type;
	// 함정 효과 ( 0 - NONE, 1 - Stun, 3 - Damage )
	public int debuff;

	// Float Data
	// 함정 피격 범위
	public float range;
	// 함정 데미지
	public float damage;
	// 함정 상태 이상 시간
	public float duration;
	// 함정 쿨타임
	public float cooldowns;
}