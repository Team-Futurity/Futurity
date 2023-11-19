using System;
using UnityEngine;

public enum PlayerState : int
{
	Idle,           // 대기

	// 콤보 공격
	AttackDelay,        // 공격 전 딜레이
	NormalAttack,       // 일반공격 
	ChargedAttack,      // 차지공격
	AttackAfterDelay,   // 공격 후 딜레이

	Hit,            // 피격
	Move,           // 이동
	Dash,           // 대시
	Death,          // 사망

	// 액티브 부품(필살기)
	BasicSM,      // 기본 부품
	BetaSM,

	// 기타 유틸
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
	SpecialAttack_Complete = 20,
	Dash = 3,
	Move = 4,
	Move_Up = 40,
	Move_Down,
	Move_Right,
	Move_Left,
	SpecialMove = 5,
	SpecialMove_Complete = 50
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