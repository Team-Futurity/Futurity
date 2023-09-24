using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStageMove : MonoBehaviour
{
	[SerializeField] private Transform playerPos;
	[SerializeField] private Transform[] movePosition;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			playerPos.position = movePosition[0].position;
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			playerPos.position = movePosition[1].position;
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			playerPos.position = movePosition[2].position;
		}
	}
}
