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
		// other의 instanceID를 저장한다. 
		// 다시 충돌 시, instanceID를 확인 후 문제 없을 경우만 처리
		// 몬스터나 플레이어와 충돌 시, 데미지를 준다.
		
		switch (other.tag)
		{
			case "Ground":
				// Ground 체크 시에는 다시 리셋 상태로 돌아간다.
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
