using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RewardBox : MonoBehaviour
{
	public UIPassivePartSelect passivePartSelect;
	public int[] partCodes;
	private bool isEnter;

	public int[] partDataBase = { 2201, 2202, 2101, 2102, 2103, 2104, 2105, 2106 };

	private void OnDisable()
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		isEnter = true;
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		isEnter = false;
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnInteractRewardBox(InputAction.CallbackContext context)
	{
		if (UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART))
		{
			return;
		}

		passivePartSelect.SetPartData(GetPlayerEquipPartList());
		UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);

		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		gameObject.SetActive(false);
	}

	private int[] GetPlayerEquipPartList()
	{
		var firstPassivePart = PlayerPrefs.GetInt("Passive0");
		var secondPassivePart = PlayerPrefs.GetInt("Passive1");
		var thirdPassivePart = PlayerPrefs.GetInt("Passive2");
		var activePart = PlayerPrefs.GetInt("ActivePart");
		
		var temp = partDataBase.ToList();
		
		RandNum(firstPassivePart, ref temp);
		RandNum(secondPassivePart, ref temp);
		RandNum(thirdPassivePart, ref temp);
		RandNum(activePart, ref temp);

		var key1 = Random.Range(0, temp.Count);
		partCodes[0] = temp[key1];
		temp.Remove(key1);
		
		var key2 = Random.Range(0, temp.Count);
		partCodes[1] = temp[key2];
		temp.Remove(key2);
		
		var key3 = Random.Range(0, temp.Count);
		partCodes[2] = temp[key3];
		temp.Remove(key3);
		
		Debug.Log(partCodes[0] + " : " + partCodes[1] + " : " + partCodes[2]);
		return partCodes;
	}

	private void RandNum(int num, ref List<int> index)
	{
		if (num == 0)
			return;

		var cindex = index.FindIndex((x) => x.Equals(num));
		index.Remove(cindex);
	}
}
