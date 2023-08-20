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

	// ��Ÿ ��ƿ
	AutoMove
}

public enum PlayerInput : int
{
	None,
	NormalAttack,
	NormalAttack_J,
	NormalAttack_JJ,
	NormalAttack_JJJ,
	SpecialAttack,
	Dash,
	Move,
	Move_Up,
	Move_Down,
	Move_Right,
	Move_Left,
	SpecialMove
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
	public PlayerInput inputState;
}