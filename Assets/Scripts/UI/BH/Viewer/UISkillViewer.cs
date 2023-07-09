using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum SkillSocketName
{
	ACTIVE = 0,
	PASSIVE_1,
	PASSIVE_2,
	PASSIVE_3,
}
public class UISkillViewer : MonoBehaviour
{
	// 0 ~ 2 : Passive
	// 3 : Only Active
	public SocketController[] socketControllers;

	private Dictionary<SkillSocketName, SocketController> socketDic =
		new Dictionary<SkillSocketName, SocketController>();

	private void Awake()
	{
		if (socketControllers is null)
		{
			FDebug.Log($"{socketControllers.GetType()}이 존재하지 않습니다.");
		}
		
		SetUp();
	}

	public void SetSocket(SkillSocketName socketName, ItemUIData uiData)
	{
		socketDic[socketName].SetItemUIData(uiData);
	}
	
	private void SetUp()
	{
		var socketNameList = Enum.GetValues(typeof(SkillSocketName)).Cast<SkillSocketName>();
		int socketList = 0;
		
		foreach (var skillName in socketNameList)
		{
			if (skillName == SkillSocketName.ACTIVE)
			{
				socketDic.Add(skillName, socketControllers[3]);
				continue;
			}
			
			socketDic.Add(skillName, socketControllers[socketList++]);
		}
	}
}