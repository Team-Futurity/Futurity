using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

public interface IFSM{ }

public class UnitFSM<Unit> : MonoBehaviour where Unit : IFSM
{
	private Dictionary<int, UnitState<Unit>> states;

	protected Unit unit;

	private UnitState<Unit> currentState;
	private UnitState<Unit> prevState;
	// subState는 currentState에 종속적으로 작동하며 currentState가 End되면 같이 End되며 null이 된다.
	private UnitState<Unit> subState; 

	protected void SetUp(ValueType firstState)
	{
		states = new Dictionary<int, UnitState<Unit>>();

		var stateTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(UnitState<Unit>).IsAssignableFrom(t));


		foreach (var stateType in stateTypes)
		{
			var attribute = stateType.GetCustomAttribute<FSMStateAttribute>();

			if (attribute == null)
			{
				continue;
			}

			var state = Activator.CreateInstance(stateType) as UnitState<Unit>;

			if (!states.TryAdd(attribute.key, state))
			{
				Debug.LogError($"[FSM ERROR] {typeof(Unit)} ??{attribute.key} ?ㅺ? 以묐났?섏뿀?듬땲??");
			}
		}

		ChangeState(firstState);
	}

	public void ChangeState(ValueType nextEnumState)
	{
		UnitState<Unit> state = null;
		if (GetState(nextEnumState, ref state))
		{
			ChangeState(state);
		}
	}

	public void ChangeState(UnitState<Unit> nextState)
	{
		if (nextState != null && nextState != currentState)
		{
			if (currentState != null)
			{
				currentState.End(unit);
				subState?.End(unit);
				subState = null;
				prevState = currentState;
			}

			currentState = nextState;
			currentState.Begin(unit);
		}
		else
		{
			FDebug.Log($"NextState({nextState})가 null이거나 현재 상태와 동일합니다.\n현재 상태 : {currentState}");
		}
	}

	public void AddSubState(ValueType subEnumState)
	{
		UnitState<Unit> state = null;
		if (GetState(subEnumState, ref state))
		{
			AddSubState(state);
			Debug.Log("SUB STATE" + state);
		}
	}

	public void AddSubState(UnitState<Unit> subState)
	{
		if (subState != null && subState != currentState)
		{
			this.subState = subState;
			this.subState.Begin(unit);
		}
		else
		{
			FDebug.Log($"NextState({subState})가 null이거나 현재 상태와 동일합니다.\n현재 상태 : {currentState}");
		}
	}

	public void RemoveSubState()
	{
		if(subState != null)
		{
			subState.End(unit);
			subState = null;
		}
	}

	public void BackToPreviousState()
	{
		if (prevState != null)
		{
			ChangeState(prevState);
		}
	}

	public bool IsCurrentState(ValueType enumState)
	{
		UnitState<Unit> state = null;
		if (GetState(enumState, ref state))
		{
			return state == currentState;
		}

		return false;
	}

	public bool GetState(ValueType enumState, ref UnitState<Unit> state)
	{
		bool isProcessed = true;
		if (typeof(int) != ((int)enumState).GetType())
		{
			FDebug.LogError($"{enumState}는 int를 상속받는 enum타입이 아닙니다.");
			isProcessed = false;
		}
		else if (!states.TryGetValue((int)enumState, out state))
		{
			FDebug.Log($"{enumState}는 찾을 수 없는 타입입니다.");
			isProcessed = false;
		}

		return isProcessed;
	}

	protected virtual void Update()
	{
		currentState?.Update(unit);
		subState?.Update(unit);
	}

	protected virtual void FixedUpdate()
	{
		currentState?.FixedUpdate(unit);
		subState?.FixedUpdate(unit);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		currentState?.OnTriggerEnter(unit, other);
		subState?.OnTriggerEnter(unit, other);
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		currentState?.OnTriggerExit(unit, other);
		subState?.OnTriggerExit(unit, other);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		currentState?.OnCollisionEnter(unit, collision);
		subState?.OnCollisionEnter(unit, collision);
	}

	protected virtual void OnCollisionStay(Collision collision)
	{
		currentState?.OnCollisionStay(unit, collision);
		subState?.OnCollisionStay(unit, collision);
	}
}
