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
	BasicSM,      // �⺻ ��ǰ

	// ��Ÿ ��ƿ
	AutoMove
}

public enum PlayerInputEnum : int
{
	None = 0,
	NormalAttack = 1,
	NormalAttack_J = 10,
	NormalAttack_JJ,
	NormalAttack_JJJ,
	NormalAttack_JJK,
	NormalAttack_JK,
	NormalAttack_JKK,
	SpecialAttack = 2,
	Dash = 3,
	Move = 4,
	Move_Up = 40,
	Move_Down,
	Move_Right,
	Move_Left,
	SpecialMove = 5
}

[Serializable]
public struct PlayerStateInfo
{
	public PlayerState stateType;
	public UnitState<PlayerController> state;
}

public struct PlayerInputData
{
	public string inputMsg;
	public PlayerInputEnum inputState;
}