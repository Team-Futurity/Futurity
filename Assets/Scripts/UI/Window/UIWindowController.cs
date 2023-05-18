using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
public class UIWindowController : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private RectTransform rectTransform;
	[SerializeField]
	public UnityEvent[] windowEvents = new UnityEvent[8];

	//#����#	Ÿ UIWindow���� �տ� ���� ������ ���� UI
	[SerializeField]
	private GameObject modalBackground;

	[SerializeField]
	public bool isLock = false;

	[SerializeField]
	private List<Button> buttons;

	[SerializeField]
	private UnityEvent closeEvent;



	public void Start()
	{
		TryGetComponent<RectTransform>(out rectTransform);

		//#����#	isLock�� true��� Ÿ UI�� �������� ���ϵ��� ����
		if (isLock)
		{
			BringToFront();
		}


		UIWindowManager.Instance.SetButtons(buttons);
	}


	public void UIWindowNewOpen(GameObject OpenUIWindowObject)
	{
		//#����#	�½�Ʈ�� ��ũ��Ʈ�� ���ο� Windowâ�� ����.
		UIWindowManager.Instance.UIWindowOpen(OpenUIWindowObject, transform.parent, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	public void UIWindowClose()
	{
		//#����#	�ڱ� �ڽ��� �ݴ´�.
		UIWindowManager.Instance.UIWindowClose(this.gameObject);
		closeEvent?.Invoke();
	}

	public void UIWindowSiblingAllClose()
	{
		//#����#	���� ��ü���� ���� �ݾƹ�����.
		UIWindowManager.Instance.UIWindowChildAllClose(transform.parent);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//#����#	�ش� UIWindow�� Ŭ���Ǿ����� ���� ������ ������� ����
		BringToFront();
	}

	public void BringToFront()
	{
		//#����#	�ش� UIWindow�� ���� ������ ������� ����
		rectTransform.SetAsLastSibling();
	}

	public void EventStarter(int eventNumber)
	{
		//#����#	�̺�Ʈ ȣ���
		windowEvents[eventNumber]?.Invoke();
		UIWindowClose();
	}

	public List<Button> GetButtons()
	{
		return buttons;
	}
}
