using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUnitManager : Singleton<InGameUnitManager>
{
	//�ӽ� : ���� ���� ����, ũ��ƽ ���带 ����
	public PlayerController player;
	public List<Enemy> enemys;

	private void Start()
	{

	}
}