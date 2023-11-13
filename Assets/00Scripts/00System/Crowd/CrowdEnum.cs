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
	
	// ����
	STUN = 3001,
	// �̵� �ӵ� ����
	SLOW = 3002,
	// ���� �ӵ� ����
	CRIPPLE = 3003,
	// ����
	INVINICIBILITY = 3004,
	// ���� ������
	DOT = 3005,
	// ü�� ȸ��
	RECOVERY = 3006,
	
	END
}