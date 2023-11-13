using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
	// Crowd Giver : ������ �ִ� ������ �����Ѵ�.
	[SerializeField]
	private List<CrowdBase> hasCrowdList = new List<CrowdBase>();
	
	// Crowd Receiver : ���� ���� ������ �����Ѵ�.
	private List<CrowdBase> recCrowdList = new List<CrowdBase>();

	public UnitBase debugTest;
	
	private void Awake()
	{
		if (debugTest != null)
		{
			SendCrowd(debugTest, 0);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if(debugTest != null)
				SendCrowd(debugTest, 1);
		}
	}

	public bool AddCrowdData(CrowdBase crowd)
	{
		// �ߺ��� ���� ���, ���Ѵ�.
		if (HasCrowd(crowd.data.CrowdName)) return false;
		
		recCrowdList.Add(crowd);
		
		return true;
	}

	public bool RemoveCrowdData(CrowdBase crowd)
	{
		if (!HasCrowd(crowd.data.CrowdName)) return false;
		
		recCrowdList.Remove(crowd);

		return true;
	}

	public bool HasCrowd(CrowdName name)
	{
		for (int i = 0; i < recCrowdList.Count; ++i)
		{
			if (recCrowdList[i].data.CrowdName == name)
				return true;
		}

		return false;
	}

	public CrowdBase GetCrowd(CrowdName name)
	{
		for (int i = 0; i < recCrowdList.Count; ++i)
		{
			if (recCrowdList[i].data.CrowdName == name)
				return recCrowdList[i];
		}

		return null;
	}
	
	#region Giver

	// Crowd ����
	public void SendCrowd(UnitBase unit, int index)
	{
		var isOkay = unit.TryGetComponent<CrowdSystem>(out var crowd);
		if(!isOkay) FDebug.Log("����", GetType());
		
		crowd.AddCrowd(hasCrowdList[index], unit);
	}

	#endregion
	
	
	#region Receiver

	public void AddCrowd(CrowdBase crowd, UnitBase unit)
	{
		if (HasCrowd(crowd.data.CrowdName))
		{
			Debug.Log(transform.name);
			
			var tempCrowd = GetCrowd(crowd.data.CrowdName);
			if (tempCrowd == null) return;
			
			tempCrowd.SetCrowdTime(crowd.data.CrowdActiveTime);
			
			return;
		}

		var copyCrowd = Instantiate(crowd, transform.position, quaternion.identity, transform);
		copyCrowd.SetData(this, unit);
		
		AddCrowdData(copyCrowd);
	}
	
	#endregion
}