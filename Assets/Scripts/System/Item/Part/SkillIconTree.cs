using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconTree : MonoBehaviour
{
	public List<SocketController> sockController;

	private void Awake()
	{
		if(sockController is null)
		{
			FDebug.Log($"{sockController.GetType()}이 존재하지 않습니다.");
		}
	}

	public void SetSocket(ItemUIData uiData, int num)
	{
		sockController[num].SetItemUIData(uiData);
	}
}
