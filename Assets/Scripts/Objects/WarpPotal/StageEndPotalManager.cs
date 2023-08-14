using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지 이동 포탈의 활성화를 위한 싱글톤 클래스입니다. 
/// (임시 스크립트로 이후 싱글톤을 사용하지 않는 방향으로 변경할 필요가 있습니다.)
/// </summary>
public class StageEndPotalManager : Singleton<StageEndPotalManager>
{
	// 워프 포탈 컨트롤러 리스트입니다.
	private List<StageEndPotalController> warpPotalControllers = new List<StageEndPotalController>();
	private LastKillController lastKillController;

	/// <summary>
	/// 새로운 워프 포탈 컨트롤러를 리스트에 추가합니다.
	/// </summary>
	/// <param name="newWarpPotalController">추가할 워프 포탈 컨트롤러입니다.</param>
	public void SetEndWarpPotal(StageEndPotalController newWarpPotalController)
	{
		warpPotalControllers.Add(newWarpPotalController);
	}

	public void SetLastKillController(LastKillController getLastKillController)
	{
		lastKillController = getLastKillController;
	}

	/// <summary>
	/// 워프 포탈 컨트롤러 리스트를 초기화합니다.
	/// </summary>
	public void ClearEndWarpPotal()
	{
		warpPotalControllers.Clear();
	}

	/// <summary>
	/// 모든 워프 포탈을 활성화합니다.
	/// </summary>
	public void ActivePotals()
	{
		lastKillController.LastKill();

		foreach (var controller in warpPotalControllers)
		{
			controller.isActiveStageEndPortal = true;
		}
	}
}
