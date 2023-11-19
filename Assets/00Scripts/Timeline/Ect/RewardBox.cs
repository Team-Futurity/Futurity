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
	public UIPartEquip partEquip;
	public int[] partCodes;
	private bool isEnter;

	[Header("Effect")] 
	[SerializeField] private GameObject boxEffect;
	
	private int[] partDataBase = { 
		2201, 2202,							// Active
		2101, 2102, 2103, 2104, 2105	// Passive
	};

	private Collider enteredPlayer;

	private void OnDisable()
	{
		if (InputActionManager.Instance == null)
		{
			return;
		}
		
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

		enteredPlayer = other;

		ChapterMoveController.Instance.EnableInteractionUI(EUIType.OPENBOX);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}

		enteredPlayer = null;

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
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
		
		startAnimation = PlayAnimation();
		StartCoroutine(startAnimation);
	}

	public void EndPlayerBoxOpen(Animator anim)
	{
		anim?.SetTrigger("CloseTheBox");
	}
	
	private IEnumerator PlayAnimation()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.OPENBOX);
		boxEffect.SetActive(false);
		boxAnimations.Play();

		enteredPlayer.GetComponentInChildren<Animator>()?.SetTrigger("OpenTheBox");

		partEquip.onEnded += EndPlayerBoxOpen;

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

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
			ChapterMoveController.Instance.DisableInteractionUI(EUIType.OPENBOX);
			
			passivePartSelect.SetPartData(GetPlayerEquipPartList());
			UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);
			gameObject.GetComponent<BoxCollider>().enabled = false;

			isInteraction = true;
		}
	}

	private int[] GetPlayerEquipPartList()
	{
		var firstPassivePart = PlayerPrefs.GetInt("PassivePart0");
		var secondPassivePart = PlayerPrefs.GetInt("PassivePart1");
		var thirdPassivePart = PlayerPrefs.GetInt("PassivePart2");
		var activePart = PlayerPrefs.GetInt("ActivePart");

		if (activePart == 0) { PlayerPrefs.SetInt("ActivePart", 2201);}
		
		var temp = partDataBase.ToList();
		
		Debug.Log(firstPassivePart + " : " + secondPassivePart + " : " + thirdPassivePart + " : " + activePart);

		// 끼고 있는거 제거
		temp = RandNum(firstPassivePart, temp);
		temp = RandNum(secondPassivePart, temp);
		temp = RandNum(thirdPassivePart, temp);
		temp = RandNum(activePart, temp);
		
		// 픽된거 제거
		var key1 = Random.Range(0, temp.Count);
		partCodes[0] = temp[key1];
		temp.Remove(temp[key1]);
		
		temp = RandNum(partCodes[0], temp);
		
		// 픽된거 제거
		var key2 = Random.Range(0, temp.Count);
		partCodes[1] = temp[key2];
		temp.Remove(temp[key2]);
		
		temp = RandNum(partCodes[1], temp);
		
		// 픽된거 제거
		var key3 = Random.Range(0, temp.Count);
		partCodes[2] = temp[key3];
		temp.Remove(temp[key3]);
		
		temp.Clear();
		
		return partCodes;
	}

	private List<int> RandNum(int num, List<int> index)
	{
		var cindex = index.FindIndex((x) => x.Equals(num));
		if(cindex != -1) index.RemoveAt(cindex);

		return index;
	}
}
