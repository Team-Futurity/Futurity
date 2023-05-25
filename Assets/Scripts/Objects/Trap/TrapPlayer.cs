using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlayer : MonoBehaviour
{
	// Code
	public int trapCode { get; private set; }

	private bool isActive;

	public TrapBehaviour trapBehaviour;
	[SerializeField] public TrapData trapData { get; private set; }


	private void Awake()
	{
		ResetTrap();
	}

	private void FixedUpdate()
	{
		if(isActive)
		{
			SearchArround();
		}
	}

	public void SearchArround()
	{
	}

	private void ActiveTrap(UnitBase unit)
	{
		trapBehaviour.ActiveTrap(unit);
	}

	private void ActiveTrap(UnitBase[] units)
	{
		trapBehaviour.ActiveTrap(units);
	}

	private void ResetTrap()
	{
		isActive = true;
		trapBehaviour.SetData();
	}

}
