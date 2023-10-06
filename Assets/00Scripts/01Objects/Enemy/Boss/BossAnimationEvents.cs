using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
	public BossController bc;

	public void ActiveEffect(int index = 0)
	{

	}

	#region Activate Attack
	public void ActivateType1Attack()
	{
		bc.Type1Collider.SetActive(true);
	}
	public void ActivateType2Attack()
	{
		bc.Type2Collider.SetActive(true);
	}
	public void ActivateType3Attack()
	{
		bc.Type3Collider.SetActive(true);
	}
	#endregion
}
