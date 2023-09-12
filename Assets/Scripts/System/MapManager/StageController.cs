using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// ��� Area ������ ������ �ֱ�
	[field: SerializeField]
	public List<AreaController> areaControllerList;

	// ���� Area�� Index
	[field: SerializeField]
	public int currentAreaIndex = 0;
	private int maxAreaIndex = 0;

	// ���� Area�� Init, Play, Stop �޼��� ������ �ֱ�
	// Init : ������ �ʱ�ȭ
	// Play : �ﰢ���� ���� Play
	// Stop : ���� �Ͻ� ���� -> UI ����

	// ���� Area�� Start Directing�� ������ ���� ��� �����Ѵ�. -> Play���� ó������ ��
	// ���� ������ �̵��� ��, SetNextArea -> ���̹� �����غ� ��.

	private void Awake()
	{
		maxAreaIndex = areaControllerList.Count;
	}

	public void SetNextArea()
	{
		// �ε��� ����
	}

	public void InitArea()
	{
	}

	public void PlayArea()
	{

	}

	public void StopArea()
	{

	}
}
