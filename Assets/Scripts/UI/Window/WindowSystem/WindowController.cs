using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
public class WindowController : MonoBehaviour
{
	[Header ("Window��� ������ ������ �־���� �ʼ� ������")]
	[Space (15)]

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

	/// <summary>
	/// �����츦 Ȱ��ȭ�ϰ�, Ȱ��ȭ�� ���õ� �̺�Ʈ�� ȣ���մϴ�.
	/// </summary>
	public void EnabledWindow()
	{
		enabledEvent?.Invoke();
		Debug.Log("EnabledWindow ����");
	}

	/// <summary>
	/// ���ο� �����츦 ����, ��ġ�� ũ�⸦ �����մϴ�.
	/// </summary>
	public void WindowNewOpen(GameObject OpenUIWindowObject)
	{
		WindowManager.Instance.WindowOpen(OpenUIWindowObject, transform.parent, true, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	/// <summary>
	/// ���� �����츦 �ݽ��ϴ�. ���� �̺�Ʈ�� ȣ���մϴ�.
	/// </summary>
	public void WindowClose()
	{
		WindowManager.Instance.WindowClose(this.gameObject);
		closeEvent?.Invoke();
	}

	/// <summary>
	/// ���� ��ü(������ �θ� ���� ��ü)���� ��� �ݽ��ϴ�.
	/// </summary>
	public void WindowSiblingAllClose()
	{
		//#����#	���� ��ü���� ���� �ݾƹ�����.
		WindowManager.Instance.WindowChildAllClose(transform.parent);
	}

	/// <summary>
	/// �ش� �����츦 ���� ������ ����ɴϴ�.
	/// </summary>
	public void BringToFront()
	{
		//#����#	�ش� UIWindow�� ���� ������ ������� ����
		rectTransform.SetAsLastSibling();
	}

	/// <summary>
	/// ������ ��ȣ�� �̺�Ʈ�� �����ϰ�, �����츦 �ݽ��ϴ�.
	/// </summary>
	public void EventStarter(int eventNumber)
	{
		//#����#	�̺�Ʈ ȣ���
		windowEvents[eventNumber]?.Invoke();
		WindowClose();
	}

	/// <summary>
	/// �� �����쿡 �ִ� ��ư ����� ��ȯ�մϴ�.
	/// </summary>
	/// <returns>�� �������� ��ư ����Ʈ�Դϴ�.</returns>
	public List<Button> GetButtons()
	{
		return buttons;
	}

	/// <summary>
	/// ������ �����ϴ� �޼ҵ��Դϴ�.
	/// </summary>
	/// <param name="name">������ �̸��Դϴ�.</param>
	/// <param name="value">������ ���Դϴ�.</param>
	public void SetVariable(string name, object value)
	{
		variables[name] = value;
	}
}
