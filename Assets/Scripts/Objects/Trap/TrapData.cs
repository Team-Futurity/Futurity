using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapData
{
	// Trap Code - Not Unique
	// ���� ������Ʈ�� ���� �ڵ�
	public int code;

	// Trap Code - Unique 
	// ���� �������� ������ �ִ� ������ ���� �ڵ�
	public string uID;

	// String
	// ���� �̸�
	public string name;

	// Int
	// ������ �ߵ� ���� ( 0 - NONE, 1 - �÷��̾� ����, 2 - �÷��̾� ���� )
	public int condition;
	// ������ Ÿ�� ( 0 - NONE, 1 - FALL, 2 - Debuff, 3 - Explosion )
	public int type;
	// ���� ȿ�� ( 0 - NONE, 1 - Stun, 3 - Damage )
	public int debuff;

	// Float Data
	// ���� �ǰ� ����
	public float range;
	// ���� ������
	public float damage;
	// ���� ���� �̻� �ð�
	public float duration;
	// ���� ��Ÿ��
	public float cooldowns;
}