using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
public class WindowController : MonoBehaviour, IPointerClickHandler
{
	[Header ("Window��� ������ ������ �־���� �ʼ� ������")]
	[Space (15)]


	[SerializeField] 
	private RectTransform rectTransform;
	[SerializeField]
	public UnityEvent[] windowEvents = new UnityEvent[8];


	[SerializeField]
	private int poolingWindowNum;
	public int thisWindowNum;

	[SerializeField]
	public bool isLock = false;

	[SerializeField]
	private List<Button> buttons;

	[SerializeField]
	private UnityEvent enabledEvent;

	[SerializeField]
	private UnityEvent closeEvent;

	[SerializeField]
	private Dictionary<string, object> variables = new Dictionary<string, object>();


	public void Start()
	{
		TryGetComponent<RectTransform>(out rectTransform);

		WindowManager.Instance.SetButtons(buttons);

		//#����#	isLock�� true��� Ÿ UI�� �������� ���ϵ��� ����
		if (isLock)
		{
			BringToFront();
		}
	}

	public void EnabledWindow()
	{
		enabledEvent?.Invoke();
		Debug.Log("EnabledWindow ����");
	}

	public void WindowCaching(GameObject OpenUIWindowObject)
	{
		poolingWindowNum = WindowManager.Instance.WindowPooling(OpenUIWindowObject);
	}

	public void WindowNewOpen()
	{
		//#����#	�½�Ʈ�� ��ũ��Ʈ�� ���ο� Windowâ�� ����.
		WindowManager.Instance.WindowOpen(poolingWindowNum, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	public void WindowClose()
	{
		//#����#	�ڱ� �ڽ��� �ݴ´�.
		WindowManager.Instance.WindowClose(thisWindowNum);
		closeEvent?.Invoke();
	}

	public void WindowSiblingAllClose()
	{
		//#����#	���� ��ü���� ���� �ݾƹ�����.
		WindowManager.Instance.WindowChildAllClose(transform.parent);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//#����#	�ش� UIWindow�� Ŭ���Ǿ����� ���� ������ ������� ����
		//BringToFront();
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
		WindowClose();
	}

	public List<Button> GetButtons()
	{
		return buttons;
	}

	public void SetVariable(string name, object value)
	{
		variables[name] = value;
	}
}
