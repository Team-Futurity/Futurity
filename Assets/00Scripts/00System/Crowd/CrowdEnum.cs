using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrowdType
{
	NONE = 0,
	
	BUFF,
	DEBUFF,
	
	END
}

public enum CrowdName
{
	NONE = 0,
	
	// 기절
	STUN = 3001,
	// 이동 속도 감소
	SLOW = 3002,
	// 공격 속도 감소
	CRIPPLE = 3003,
	// 무적
	INVINICIBILITY = 3004,
	// 지속 데미지
	DOT = 3005,
	// 체력 회복
	RECOVERY = 3006,
	
	END
}