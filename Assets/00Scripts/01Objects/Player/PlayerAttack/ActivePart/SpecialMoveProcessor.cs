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

		// PlayerController�� Next State : Null
		UnitState<PlayerController> nextState = null;
		
		if(!pc.GetState(stateToChange, ref nextState)) { FDebug.LogError("[ActivePartProccessor] An Error occurred in GetState"); return; }

		// Part Data�� ��� ���� Baisc�� BasicActivePart���� ����
		GetPartData();
		
		// ������ Basic Active Part�� �����Ǿ� �־ ���Ŀ��� Type�� ������ �����ؾ� �� �ʿ䰡 ������.
		var state = nextState as PlayerSpecialMoveState<BetaActivePart>;
		state.SetActivePartData(proccessor as BetaActivePart);
		pc.ChangeState(PlayerState.BetaSM);
	}
}
