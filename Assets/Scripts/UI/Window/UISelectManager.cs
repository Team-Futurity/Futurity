using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UISelectManager : MonoBehaviour
{
	//#����#	�ν����Ϳ��� �� InputActionReference�� �Ҵ����ּ���.
	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;

	//#����#	�ν����Ϳ��� ������ button���� List�� �Ҵ����ּ���.
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
		//#����#	�� �׼ǿ� ���� �̺�Ʈ �ڵ鷯�� ����մϴ�.
		leftAction.action.performed += _ => { Debug.Log("Left action performed"); SelectPreviousButton(); };
		rightAction.action.performed += _ => { Debug.Log("Right action performed"); SelectNextButton(); };
		selectAction.action.performed += _ => { Debug.Log("Select action performed"); ClickCurrentButton(); };

		leftAction.action.Enable();
		rightAction.action.Enable();
		selectAction.action.Enable();
	}

	private void OnDisable()
	{
		//#����#	�� �׼��� �̺�Ʈ �ڵ鷯�� �����մϴ�.
		leftAction.action.performed -= _ => { Debug.Log("Left action performed"); SelectPreviousButton(); };
		rightAction.action.performed -= _ => { Debug.Log("Right action performed"); SelectNextButton(); };
		selectAction.action.performed -= _ => { Debug.Log("Select action performed"); ClickCurrentButton(); };

		leftAction.action.Disable();
		rightAction.action.Disable();
		selectAction.action.Disable();
	}

	private void SelectPreviousButton()
	{
		//#����#	���� ���õ� ��ư�� ù ��° ��ư�� �ƴ� ��� ���� ��ư�� �����մϴ�.
		if (currentButtonIndex > 0)
		{
			StartCoroutine(NavigateWithDelay(() => SelectButton(currentButtonIndex - 1)));
		}
	}

	private void SelectNextButton()
	{
		//#����#	���� ���õ� ��ư�� ������ ��ư�� �ƴ� ��� ���� ��ư�� �����մϴ�.
		if (currentButtonIndex < buttons.Count - 1 && canNavigate)
		{
			StartCoroutine(NavigateWithDelay(() => SelectButton(currentButtonIndex + 1)));
		}
	}

	private void ClickCurrentButton()
	{
		//#����#	���� ���õ� ��ư�� Ŭ���մϴ�.
		Button currentButton = buttons[currentButtonIndex];
		ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
	}

	private void SelectButton(int index)
	{
		//#����#	�־��� �ε����� ��ư�� �����մϴ�.
		currentButtonIndex = index;
		currentButton = buttons[currentButtonIndex].gameObject;
		EventSystem.current.SetSelectedGameObject(currentButton);
	}

	private IEnumerator NavigateWithDelay(System.Action action)
	{
		//#����#	������ �Լ��� �����ϰ� 0.1�� ���� �׺���̼��� ��Ȱ��ȭ�մϴ�.
		canNavigate = false;
		action.Invoke();
		yield return new WaitForSeconds(0.1f);
		canNavigate = true;
	}
}
