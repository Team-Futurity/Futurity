using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUnitManager : Singleton<InGameUnitManager>
{
	//임시 : 추후 삭제 예정, 크리틱 빌드를 위함
	public PlayerController player;
	public List<Enemy> enemys;

	private void Start()
	{

	}
}