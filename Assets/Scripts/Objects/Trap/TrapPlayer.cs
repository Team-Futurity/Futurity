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
		var test = Physics.OverlapSphere(transform.position, trapData.range, 1 << 9);
	}

	private void ResetTrap()
	{
		isActive = true;
	}

}
