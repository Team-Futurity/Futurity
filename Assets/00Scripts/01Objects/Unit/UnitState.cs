using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AttributeUsage(AttributeTargets.Class)]
public class FSMStateAttribute : Attribute
{
	public readonly int key;

	public FSMStateAttribute(int key)
	{
		this.key = key;
	}
}

public abstract class UnitState<T> where T : IFSM
{
	public readonly StateData stateData;

	public UnitState() { }
	public UnitState(StateData stateData)
	{
		this.stateData = stateData;
	}

	public virtual bool IsChangable(T Unit, UnitState<T> nextState) { return true; }
	public abstract void Begin(T unit);
	public abstract void Update(T unit);
	public abstract void FixedUpdate(T unit);
	public abstract void End(T unit);
	public abstract void OnTriggerEnter(T unit, Collider other);
	public abstract void OnCollisionEnter(T unit, Collision collision);

	public virtual void OnCollisionStay(T unit, Collision collision){ }
}

