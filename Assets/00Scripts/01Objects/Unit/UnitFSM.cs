using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public interface IFSM{ }

public class UnitFSM<Unit> : MonoBehaviour where Unit : IFSM
{
	private Dictionary<int, UnitState<Unit>> states;

	protected Unit unit;

	private UnitState<Unit> currentState;
	private UnitState<Unit> prevState;
	// subState는 currentState에 종속적으로 작동하며 currentState가 End되면 같이 End되며 null이 된다.
	private UnitState<Unit> subState;

	public List<StateData> stateDatas;
	public Dictionary<int, StateData> stateDataDictionary;

	public StateChangeConditions stateChangeConditions;

	#region Setup
	protected void SetUp(ValueType firstState)
	{
		states = new Dictionary<int, UnitState<Unit>>();

		SetStateDataDictionary(stateDatas);

		var stateTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(UnitState<Unit>).IsAssignableFrom(t));


		foreach (var stateType in stateTypes)
		{
			var attribute = stateType.GetCustomAttribute<FSMStateAttribute>();

			if (attribute == null)
			{
				continue;
			}

			StateData stateData;
			stateDataDictionary.TryGetValue(attribute.key, out stateData);


			UnitState<Unit> state;
			if (stateData == null)
			{
				state = Activator.CreateInstance(stateType) as UnitState<Unit>;
			}
			else
			{
				state = Activator.CreateInstance(stateType, new object[] { stateData }) as UnitState<Unit>;
			}

			if (!states.TryAdd(attribute.key, state))
			{
				Debug.LogError($"[FSM ERROR] {typeof(Unit)}의 State 중 {attribute.key}번째 State를 Add하는 데 실패했습니다. 이미 추가했을 수 있습니다.");
			}
		}

		ChangeState(firstState);
	}

	protected void SetUp(ValueType firstState, List<ValueType> statesToUse) 
	{
		if(statesToUse.Count == 0) { FDebug.LogWarning("[UnitFSM] Count of List<ValueType> stateToUse is Zero. If you don't want that, Check this Method Call Part"); return; }

		states = new Dictionary<int, UnitState<Unit>>();

		SetStateDataDictionary(stateDatas);

		var stateTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(UnitState<Unit>).IsAssignableFrom(t));

		foreach (var stateType in stateTypes)
		{
			var attribute = stateType.GetCustomAttribute<FSMStateAttribute>();

			if (attribute == null)
			{
				continue;
			}

			var correctState = statesToUse.Where(value => (int)value == attribute.key);
			if (correctState.Count() == 0)
			{
				continue;
			}

			StateData stateData;
			stateDataDictionary.TryGetValue(attribute.key, out stateData);

			var state = Activator.CreateInstance(stateType, stateData) as UnitState<Unit>;

			if (!states.TryAdd(attribute.key, state))
			{
				Debug.LogError($"[FSM ERROR] {typeof(Unit)}의 State 중 {attribute.key}번째 State를 Add하는 데 실패했습니다. 이미 추가했을 수 있습니다.");
			}
		}

		ChangeState(firstState);
	}

	private void SetStateDataDictionary(List<StateData> list)
	{
		stateDataDictionary = new Dictionary<int, StateData>();

		foreach(StateData data in list)
		{
			if (stateDataDictionary.ContainsKey(data.enumNumber)) { continue; }

			stateDataDictionary.Add(data.enumNumber, data);
		}
	}
	#endregion

	#region State Changes
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
			int currentStateIndex = GetStateOrder(currentState);
			int nextStateIndex = GetStateOrder(nextState);

			if (currentState != null)
			{
				if (stateChangeConditions != null &&
					!stateChangeConditions.GetChangable(currentStateIndex, nextStateIndex) || !currentState.IsChangable(unit, nextState))
				{
					//FDebug.Log($"CurrentState cannot change to {nextState}", GetType());
					return;
				}

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

	public void BackToPreviousState()
	{
		if (prevState != null)
		{
			ChangeState(prevState);
		}
	}
	#endregion

	#region SubState
	public void AddSubState(ValueType subEnumState)
	{
		UnitState<Unit> state = null;
		if (GetState(subEnumState, ref state))
		{
			AddSubState(state);
		}
	}

	public void AddSubState(UnitState<Unit> subState)
	{
		if(this.subState != null) { FDebug.Log($"현재 SubState({subState})가 존재합니다. RemoveSubState()로 삭제하세요"); }

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
	#endregion

	#region Getter
	public bool IsChangableState(ValueType nextEnumState)
	{
		UnitState<Unit> state = null;
		if (GetState(nextEnumState, ref state))
		{
			return IsChangableState(state);
		}

		return false;
	}

	public bool IsChangableState(UnitState<Unit> nextState)
	{
		int currentStateIndex = GetStateOrder(currentState);
		int nextStateIndex = GetStateOrder(nextState);

		if (currentState == null || nextState == null) { return false; }
		if (stateChangeConditions == null) { return false; }

		return stateChangeConditions.GetChangable(currentStateIndex, nextStateIndex) && currentState.IsChangable(unit, nextState);
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

	public int GetStateOrder(UnitState<Unit> state)
	{
		foreach(var pair in states)
		{
			if(pair.Value == state)
			{
				return pair.Key;
			}
		}

		return -1;
	}
	#endregion

	#region UnityMethods
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
	#endregion
}
