using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INIT_AREA : Stage���� ó������ ����Ǵ� Area
// LAST_AREA : Stage�� ������ Area
// NORMAL_AREA : �� �ΰ��� Type�� �ƴ� Area

public enum AreaType
{
	NONE = 0,

	INIT_AREA,
	NORMAL_AREA,
	LAST_AREA,

	MAX
}