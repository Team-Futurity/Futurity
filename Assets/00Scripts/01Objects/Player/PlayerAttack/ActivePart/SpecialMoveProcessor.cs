using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class SpecialMoveProcessor
{
	public PlayerState stateToChange;
	public abstract void GetPartData();
	public virtual void RunSpecialMove<Proccessor>(PlayerController pc, Proccessor proccessor) where Proccessor : SpecialMoveProcessor
	{
		if (pc == null) { FDebug.LogError("[ActivePartProccessor] pc is null"); return; }

		// PlayerController의 Next State : Null
		UnitState<PlayerController> nextState = null;
		
		if(!pc.GetState(stateToChange, ref nextState)) { FDebug.LogError("[ActivePartProccessor] An Error occurred in GetState"); return; }

		// Part Data를 들고 오고 Baisc은 BasicActivePart에서 들고옴
		GetPartData();
		
		// 지금은 Basic Active Part로 설정되어 있어서 추후에는 Type을 가지고 변경해야 할 필요가 존재함.
		var state = nextState as PlayerSpecialMoveState<BetaActivePart>;
		state.SetActivePartData(proccessor as BetaActivePart);
		pc.ChangeState(PlayerState.BetaSM);
	}
}
