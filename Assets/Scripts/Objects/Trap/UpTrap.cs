using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrap : TrapBehaviour
{
	protected override void ActiveTrap()
	{
		// trap이 실행되었을 경우

		var objList = SearchAround();

		// ObjList에서 몬스터와 Player를 구분한다.
		foreach(var obj in objList)
		{
			if(obj.CompareTag("Player"))
			{
				obj.GetComponent<Rigidbody>().velocity += new Vector3(0f, 3f, 0f);
			}

			// OBJ 기절

			// 버프 발동 시간이 끝나면 데미지
			obj.GetComponent<UnitBase>().Hit(this, trapData.damage);
		}
	}

}
