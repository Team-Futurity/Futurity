using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapData
{
	// Trap Code - Unique 
	// ���� �������� ������ �ִ� ������ ���� �ڵ�
	// ���� �߰� ���ɼ��� �ִٰ� �Ǵ��Ͽ� ���� ������
	[field: SerializeField] public string TrapUID { get; private set; }
	
	
	// Trap Code - Not Unique
	// ���� ������Ʈ�� ���� �ڵ�
	[field: SerializeField] public int TrapCode { get; private set; }

	// String
	// ���� �̸�
	[field: SerializeField] public string TrapName { get; private set; }

	// Int
	// ������ �ߵ� ���� ( 0 - NONE, 1 - �÷��̾� ����, 2 - �÷��̾� ���� )
	[field: SerializeField] public TrapCondition TrapCondition { get; private set; } 
	
	// ������ Ÿ�� ( 0 - NONE, 1 - FALL, 2 - Debuff, 3 - Explosion )
	[field: SerializeField] public TrapType TrapType { get; private set; }

	// ���� ȿ�� ( 0 - NONE, 1 - Stun, 3 - Damage )
	[field: SerializeField] public int TrapDebuff { get; private set; }

	// Float Data
	// ���� �ǰ� ����
	[field: SerializeField] public float TrapRange { get; private set; }
	
	// ���� ������
	[field: SerializeField] public float TrapDamage { get; private set; }
	
	// ���� ���� �̻� �ð�
	[field: SerializeField] public float TrapDuration { get; private set; }
	
	// ���� ��Ÿ��
	[field: SerializeField] public float TrapCooldowns { get; private set; }
}