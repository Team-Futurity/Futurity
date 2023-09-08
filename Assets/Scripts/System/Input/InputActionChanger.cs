using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputActionChanger : MonoBehaviour
{
	[SerializeField] private List<InputActionData> actionDatas;
	//[SerializeField] private 

	public void SetInputActionAsset(InputActionType type)
	{
		InputActionData data = actionDatas.First(data => data.actionType == type);

		
	}
}
