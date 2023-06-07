using System;
using UnityEngine;

public enum PlayerState : int
{
	Idle,           // ���

	// �޺� ����
	AttackDelay,        // ���� �� ������
	NormalAttack,       // �Ϲݰ��� 
	ChargedAttack,      // ��������
	AttackAfterDelay,   // ���� �� ������

	Hit,            // �ǰ�
	Move,           // �̵�
	Dash,           // ���
	Death,          // ���

	// ��Ƽ�� ��ǰ(�ʻ��)
	BasicPart,      // �⺻ ��ǰ
}

public enum PlayerInput : int
{
	None,
	NormalAttack,
	SpecialAttack,
	Dash
}

[Serializable]
public struct PlayerStateInfo
{
	public PlayerState stateType;
	public UnitState<PlayerController> state;
}