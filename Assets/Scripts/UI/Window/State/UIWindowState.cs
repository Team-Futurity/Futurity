using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WindowState
{
	NONE = 0,

	UNASSIGN,
	ASSIGN,
	ACTIVE,

	MAX
}

// UNASSIGN = WindowManager에서 등록이 해제된 상태
// ASSIGN = WindowManager에 등록된 상태
// ACTIVE = WindowManager에 등록되고, 보여지고 있는 상태