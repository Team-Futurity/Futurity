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

		UnitState<PlayerController> nextState = null;
		
		if(!pc.GetState(stateToChange, ref nextState)) { FDebug.LogError("[ActivePartProccessor] An Error occurred in GetState"); return; }

		GetPartData();
		var state = nextState as PlayerSpecialMoveState<BasicActivePart>;
		state.SetActivePartData(proccessor as BasicActivePart);
		pc.ChangeState(PlayerState.BasicSM);
	}
}
