using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


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
			modalBackground = UIWindowManager.Instance.CreateModalBackground(gameObject);

			RectTransform modalRectTransform = modalBackground.GetComponent<RectTransform>();
			modalRectTransform.SetAsLastSibling();
			modalBackground.GetComponent<Image>().raycastTarget = false;

			BringToFront();
		}


		UIWindowManager.Instance.SetButtons(buttons);
	}


	//#����#	�½�Ʈ�� ��ũ��Ʈ�� ���ο� Windowâ�� ����.
	public void UIWindowNewOpen(GameObject OpenUIWindowObject)
	{
		UIWindowManager.Instance.UIWindowOpen(OpenUIWindowObject, transform.parent, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	//#����#	�ڱ� �ڽ��� �ݴ´�.
	public void UIWindowClose()
	{
		UIWindowManager.Instance.UIWindowClose(this.gameObject);
		closeEvent?.Invoke();
	}

	//#����#	���� ��ü���� ���� �ݾƹ�����.
	public void UIWindowSiblingAllClose()
	{
		UIWindowManager.Instance.UIWindowChildAllClose(transform.parent);
	}

	//#����#	�ش� UIWindow�� Ŭ���Ǿ����� ���� ������ ������� ����
	public void OnPointerClick(PointerEventData eventData)
	{
		BringToFront();
	}

	//#����#	�ش� UIWindow�� ���� ������ ������� ����
	public void BringToFront()
	{
		rectTransform.SetAsLastSibling();
	}

	//#����#	�̺�Ʈ ȣ���
	public void EventStarter(int eventNumber)
	{
		windowEvents[eventNumber]?.Invoke();
	}

	public List<Button> GetButtons()
	{
		return buttons;
	}
}
