using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INIT_AREA : Stage에서 처음으로 실행되는 Area
// LAST_AREA : Stage의 마지막 Area
// NORMAL_AREA : 위 두개의 Type이 아닌 Area

public enum AreaType
{
	NONE = 0,

	INIT_AREA,
	NORMAL_AREA,
	LAST_AREA,

	MAX
}