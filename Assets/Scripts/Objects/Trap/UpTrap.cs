using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrap : TrapBehaviour
{
	protected override void ActiveTrap()
	{
		// trap�� ����Ǿ��� ���

		var objList = SearchAround();

		// ObjList���� ���Ϳ� Player�� �����Ѵ�.
		foreach(var obj in objList)
		{
			if(obj.CompareTag("Player"))
			{
				obj.GetComponent<Rigidbody>().velocity += new Vector3(0f, 3f, 0f);
			}

			// OBJ ����

			// ���� �ߵ� �ð��� ������ ������
			obj.GetComponent<UnitBase>().Hit(this, trapData.damage);
		}
	}

}
