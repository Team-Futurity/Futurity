using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrapFallObject : MonoBehaviour
{

	public void StartFall()
	{
		gameObject.SetActive(true);
	}

	public void OnCollisionEnter(Collision coll)
	{
		// 플레이어나 몬스터 맞으면 오브젝트 삭제

		// 안맞으면 지면에 멈추고 오브젝트 삭제
	}
}
