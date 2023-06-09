using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


//#설명#	해당 스크립트는 UIWindow가 가지고 있는 핵심 스크립트로, UIWindow의 전반적인 행동 양식을 지니고 있습니다.
public class WindowController : MonoBehaviour
{
	[Header ("Window라면 무조건 가지고 있어야할 필수 관리자")]
	[Space (15)]

	[SerializeField] 
	private RectTransform rectTransform;
	[SerializeField]
	public UnityEvent[] windowEvents = new UnityEvent[8];

	//#설명#	타 UIWindow보다 앞에 나와 간섭을 막는 UI
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

		//#설명#	isLock이 true라면 타 UI를 간섭하지 못하도록 막음
		if (isLock)
		{
			BringToFront();
		}
	}

	/// <summary>
	/// 윈도우를 활성화하고, 활성화에 관련된 이벤트를 호출합니다.
	/// </summary>
	public void EnabledWindow()
	{
		enabledEvent?.Invoke();
		Debug.Log("EnabledWindow 가동");
	}

	/// <summary>
	/// 새로운 윈도우를 열고, 위치와 크기를 설정합니다.
	/// </summary>
	public void WindowNewOpen(GameObject OpenUIWindowObject)
	{
		WindowManager.Instance.WindowOpen(OpenUIWindowObject, transform.parent, true, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	/// <summary>
	/// 현재 윈도우를 닫습니다. 닫힘 이벤트를 호출합니다.
	/// </summary>
	public void WindowClose()
	{
		WindowManager.Instance.WindowClose(this.gameObject);
		closeEvent?.Invoke();
	}

	/// <summary>
	/// 형제 객체(동일한 부모를 가진 객체)들을 모두 닫습니다.
	/// </summary>
	public void WindowSiblingAllClose()
	{
		//#설명#	형제 객체들을 전부 닫아버린다.
		WindowManager.Instance.WindowChildAllClose(transform.parent);
	}

	/// <summary>
	/// 해당 윈도우를 가장 앞으로 끌어옵니다.
	/// </summary>
	public void BringToFront()
	{
		//#설명#	해당 UIWindow를 가장 앞으로 끌고오는 역할
		rectTransform.SetAsLastSibling();
	}

	/// <summary>
	/// 지정된 번호의 이벤트를 시작하고, 윈도우를 닫습니다.
	/// </summary>
	public void EventStarter(int eventNumber)
	{
		//#설명#	이벤트 호출기
		windowEvents[eventNumber]?.Invoke();
		WindowClose();
	}

	/// <summary>
	/// 이 윈도우에 있는 버튼 목록을 반환합니다.
	/// </summary>
	/// <returns>이 윈도우의 버튼 리스트입니다.</returns>
	public List<Button> GetButtons()
	{
		return buttons;
	}

	/// <summary>
	/// 변수를 설정하는 메소드입니다.
	/// </summary>
	/// <param name="name">변수의 이름입니다.</param>
	/// <param name="value">변수의 값입니다.</param>
	public void SetVariable(string name, object value)
	{
		variables[name] = value;
	}
}
