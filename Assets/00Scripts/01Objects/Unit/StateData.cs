using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StateData : ScriptableObject
{
	public Type stateType;

	public virtual void SetDataToState<T>(UnitFSM<T> fsm) where T : IFSM { }
}
