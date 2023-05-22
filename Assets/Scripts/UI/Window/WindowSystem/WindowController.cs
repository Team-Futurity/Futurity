using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


//#설명#	해당 스크립트는 UIWindow가 가지고 있는 핵심 스크립트로, UIWindow의 전반적인 행동 양식을 지니고 있습니다.
public class WindowController : MonoBehaviour, IPointerClickHandler
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


	public void WindowNewOpen(GameObject OpenUIWindowObject)
	{
		//#설명#	태스트용 스크립트로 새로운 Window창을 연다.
		WindowManager.Instance.WindowOpen(OpenUIWindowObject, transform.parent, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	public void WindowClose()
	{
		//#설명#	자기 자신을 닫는다.
		WindowManager.Instance.WindowClose(this.gameObject);
		closeEvent?.Invoke();
	}

	public void WindowSiblingAllClose()
	{
		//#설명#	형제 객체들을 전부 닫아버린다.
		WindowManager.Instance.WindowChildAllClose(transform.parent);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//#설명#	해당 UIWindow가 클릭되었을때 가장 앞으로 끌고오는 역할
		BringToFront();
	}

	public void BringToFront()
	{
		//#설명#	해당 UIWindow를 가장 앞으로 끌고오는 역할
		rectTransform.SetAsLastSibling();
	}

	public void EventStarter(int eventNumber)
	{
		//#설명#	이벤트 호출기
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
