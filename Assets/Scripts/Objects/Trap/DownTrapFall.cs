using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrapFall : MonoBehaviour
{
	private bool isFall = false;
	private List<int> detectObj = new List<int>();
	
	private void OnTriggerEnter(Collider other)
	{
		// other�� instanceID�� �����Ѵ�. 
		// �ٽ� �浹 ��, instanceID�� Ȯ�� �� ���� ���� ��츸 ó��
		// ���ͳ� �÷��̾�� �浹 ��, �������� �ش�.
		
		switch (other.tag)
		{
			case "Ground":
				// Ground üũ �ÿ��� �ٽ� ���� ���·� ���ư���.
				break;
			
			case "Player":
				break;
			
			case "Monster":
				break;
		}
	}
	
	public void SetPos(Vector3 pos)
	{
		transform.position = pos;
	}

	public void StartFall()
	{
		if (!isFall)
		{
			isFall = true;
			
			gameObject.SetActive(isFall);
		}
	}

	public bool GetFalling()
	{
		return isFall;
	}

	private void EndFall()
	{
		if (isFall)
		{
			isFall = false;
			
			gameObject.SetActive(isFall);
		}
	}
	
}
