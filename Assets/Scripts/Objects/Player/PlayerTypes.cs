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
	BasicPart,      // 기본 부품

	// 기타 유틸
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