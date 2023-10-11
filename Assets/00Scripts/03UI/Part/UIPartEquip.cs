using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPartEquip : MonoBehaviour
{
	// 0, 1, 2 -> Top, Middle, Bottom
	public List<UIPartSelectButton> passiveButton;

	public UIPassiveSelectData dummyData1;
	public UIPassiveSelectData dummyData2;
	public UIPassiveSelectData dummyData3;

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			passiveButton[0].ChangeResource(dummyData1);
			passiveButton[1].ChangeResource(dummyData2);
			passiveButton[2].ChangeResource(dummyData3);
		}
	}

	public void SetPartData()
	{

	}
}
