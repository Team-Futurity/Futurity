using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartBehaviour : MonoBehaviour
{
	private PartItemData partItemData;

	public void SetData(PartItemData data)
	{
		partItemData = data;
	}

	public abstract void OnAction(UnitBase unit);

}
