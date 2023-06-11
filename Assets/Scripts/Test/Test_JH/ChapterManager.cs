using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : Singleton<ChapterManager>
{
	private List<StageEndPotalController> warpPotalControllers = new List<StageEndPotalController>();

	public void SetEndWarpPotal(StageEndPotalController newWarpPotalController)
	{
		warpPotalControllers.Add(newWarpPotalController);
	}

	public void ClearEndWarpPotal()
	{
		warpPotalControllers.Clear();
	}

	public void ActivePotals()
	{
		foreach (var controller in warpPotalControllers)
		{
			controller.isActiveStageEndPortal = true;
		}
	}
}
