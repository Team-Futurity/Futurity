using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �̵� ��Ż�� Ȱ��ȭ�� ���� �̱��� Ŭ�����Դϴ�. 
/// (�ӽ� ��ũ��Ʈ�� ���� �̱����� ������� �ʴ� �������� ������ �ʿ䰡 �ֽ��ϴ�.)
/// </summary>
public class StageEndPotalManager : Singleton<StageEndPotalManager>
{
	// ���� ��Ż ��Ʈ�ѷ� ����Ʈ�Դϴ�.
	private List<StageEndPotalController> warpPotalControllers = new List<StageEndPotalController>();
	private LastKillController lastKillController;

	/// <summary>
	/// ���ο� ���� ��Ż ��Ʈ�ѷ��� ����Ʈ�� �߰��մϴ�.
	/// </summary>
	/// <param name="newWarpPotalController">�߰��� ���� ��Ż ��Ʈ�ѷ��Դϴ�.</param>
	public void SetEndWarpPotal(StageEndPotalController newWarpPotalController)
	{
		warpPotalControllers.Add(newWarpPotalController);
	}

	public void SetLastKillController(LastKillController getLastKillController)
	{
		lastKillController = getLastKillController;
	}

	/// <summary>
	/// ���� ��Ż ��Ʈ�ѷ� ����Ʈ�� �ʱ�ȭ�մϴ�.
	/// </summary>
	public void ClearEndWarpPotal()
	{
		warpPotalControllers.Clear();
	}

	/// <summary>
	/// ��� ���� ��Ż�� Ȱ��ȭ�մϴ�.
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
