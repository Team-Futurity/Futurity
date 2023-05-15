using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UISelectManager : MonoBehaviour
{
	//#설명#	인스팩터에서 각 InputActionReference를 할당해주세요.
	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;

	//#설명#	인스펙터에서 선택할 button들을 List에 할당해주세요.
	public List<Button> buttons;

	[SerializeField]
	private int currentButtonIndex;
	[SerializeField]
	private GameObject currentButton;
	private bool canNavigate = true;

	private void Start()
	{
		SelectButton(0);
	}

	private void OnEnable()
	{
		//#설명#	각 액션에 대한 이벤트 핸들러를 등록합니다.
		leftAction.action.performed += _ => { Debug.Log("Left action performed"); SelectPreviousButton(); };
		rightAction.action.performed += _ => { Debug.Log("Right action performed"); SelectNextButton(); };
		selectAction.action.performed += _ => { Debug.Log("Select action performed"); ClickCurrentButton(); };

		leftAction.action.Enable();
		rightAction.action.Enable();
		selectAction.action.Enable();
	}

	private void OnDisable()
	{
		//#설명#	각 액션의 이벤트 핸들러를 제거합니다.
		leftAction.action.performed -= _ => { Debug.Log("Left action performed"); SelectPreviousButton(); };
		rightAction.action.performed -= _ => { Debug.Log("Right action performed"); SelectNextButton(); };
		selectAction.action.performed -= _ => { Debug.Log("Select action performed"); ClickCurrentButton(); };

		leftAction.action.Disable();
		rightAction.action.Disable();
		selectAction.action.Disable();
	}

	private void SelectPreviousButton()
	{
		//#설명#	현재 선택된 버튼이 첫 번째 버튼이 아닌 경우 이전 버튼을 선택합니다.
		if (currentButtonIndex > 0)
		{
			StartCoroutine(NavigateWithDelay(() => SelectButton(currentButtonIndex - 1)));
		}
	}

	private void SelectNextButton()
	{
		//#설명#	현재 선택된 버튼이 마지막 버튼이 아닌 경우 다음 버튼을 선택합니다.
		if (currentButtonIndex < buttons.Count - 1 && canNavigate)
		{
			StartCoroutine(NavigateWithDelay(() => SelectButton(currentButtonIndex + 1)));
		}
	}

	private void ClickCurrentButton()
	{
		//#설명#	현재 선택된 버튼을 클릭합니다.
		Button currentButton = buttons[currentButtonIndex];
		ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
	}

	private void SelectButton(int index)
	{
		//#설명#	주어진 인덱스의 버튼을 선택합니다.
		currentButtonIndex = index;
		currentButton = buttons[currentButtonIndex].gameObject;
		EventSystem.current.SetSelectedGameObject(currentButton);
	}

	private IEnumerator NavigateWithDelay(System.Action action)
	{
		//#설명#	지정된 함수를 실행하고 0.1초 동안 네비게이션을 비활성화합니다.
		canNavigate = false;
		action.Invoke();
		yield return new WaitForSeconds(0.1f);
		canNavigate = true;
	}
}
