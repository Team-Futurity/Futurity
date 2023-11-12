using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RewardBox : MonoBehaviour
{
	[Header("컷신 재생")] 
	[SerializeField] private bool isPlayCutScene;
	[SerializeField] private UnityEvent cutSceneEvent;
	public void IsPlayCutScene(bool active) => isPlayCutScene = active;
	
	[Header("부품 컴포넌트")]
	public UIPassivePartSelect passivePartSelect;
	public int[] partCodes;
	private bool isEnter;

	[Header("Effect")] 
	[SerializeField] private GameObject boxEffect;
	
	private int[] partDataBase = { 
		2201, 2202,							// Active
		2101, 2102, 2103, 2104, 2105	// Passive
	};

	private void OnDisable()
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	[Header("Open Animation")] 
	[SerializeField] private Animation boxAnimations;
	[SerializeField] private float waitTime = 0.6f;
	private IEnumerator startAnimation;

	[Header("상호작용 정보 저장")] 
	[ReadOnly(false)] public bool isInteraction = false;

	public void EnableRewardBox()
	{
		boxEffect.SetActive(true);
		gameObject.GetComponent<BoxCollider>().enabled = true;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		ChapterMoveController.Instance.EnableInteractionUI(EUIType.OPENBOX);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.OPENBOX);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnInteractRewardBox(InputAction.CallbackContext context)
	{
		if (isPlayCutScene == true)
		{
			cutSceneEvent?.Invoke();
			ChapterMoveController.Instance.DisableInteractionUI(EUIType.OPENBOX);
			return;
		}
		
		startAnimation = PlayAnimation();
		StartCoroutine(startAnimation);
	}
	
	private IEnumerator PlayAnimation()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.OPENBOX);
		boxEffect.SetActive(false);
		boxAnimations.Play();
		
		yield return new WaitForSeconds(waitTime);
		
		if (UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART))
		{ 
			yield return null;
		}

		passivePartSelect.SetPartData(GetPlayerEquipPartList());
		UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);
		gameObject.GetComponent<BoxCollider>().enabled = false;

		isInteraction = true;
	}

	private int[] GetPlayerEquipPartList()
	{
		var firstPassivePart = PlayerPrefs.GetInt("Passive0");
		var secondPassivePart = PlayerPrefs.GetInt("Passive1");
		var thirdPassivePart = PlayerPrefs.GetInt("Passive2");
		var activePart = PlayerPrefs.GetInt("ActivePart");

		if (activePart == 0) { PlayerPrefs.SetInt("ActivePart", 2201);}
		
		var temp = partDataBase.ToList();
		
		RandNum(firstPassivePart, ref temp);
		RandNum(secondPassivePart, ref temp);
		RandNum(thirdPassivePart, ref temp);
		RandNum(activePart, ref temp);
		
		var key1 = Random.Range(0, temp.Count);
		partCodes[0] = temp[key1];
		temp.Remove(key1);
		
		RandNum(partCodes[0], ref temp);
		
		var key2 = Random.Range(0, temp.Count);
		partCodes[1] = temp[key2];
		temp.Remove(key2);
		
		RandNum(partCodes[1], ref temp);

		var key3 = Random.Range(0, temp.Count);
		partCodes[2] = temp[key3];
		temp.Remove(key3);
		
		temp.Clear();
		
		return partCodes;
	}

	private void RandNum(int num, ref List<int> index)
	{
		var cindex = index.FindIndex((x) => x.Equals(num));
		if(cindex != -1) index.RemoveAt(cindex);
	}
}
