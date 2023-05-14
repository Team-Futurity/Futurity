using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


//#설명#	해당 스크립트는 UIWindow가 가지고 있는 핵심 스크립트로, UIWindow의 전반적인 행동 양식을 지니고 있습니다.
public class UIWindowController : MonoBehaviour, IPointerClickHandler
{
	public GameObject parentObject;
	[SerializeField]
	private RectTransform rectTransform;
	[SerializeField]
	public UnityEvent[] windowEvents = new UnityEvent[8];

	//#설명#	타 UIWindow보다 앞에 나와 간섭을 막는 UI
	[SerializeField]
	private GameObject modalBackground;

	[SerializeField]
	public bool isLock = false;


	public void Start()
	{
		TryGetComponent<RectTransform>(out rectTransform);

		if (isLock)
		{
			modalBackground = UIWindowManager.Instance.CreateModalBackground(gameObject);

			RectTransform modalRectTransform = modalBackground.GetComponent<RectTransform>();
			modalRectTransform.SetAsLastSibling();
			modalBackground.GetComponent<Image>().raycastTarget = false;

			BringToFront();
		}
	}


	//#설명#	태스트용 스크립트로 새로운 Window창을 연다.
	public void UIWindowNewOpen(GameObject OpenUIWindowObject)
	{
		UIWindowManager.Instance.UIWindowOpen(OpenUIWindowObject, transform.parent, rectTransform.localPosition + new Vector3(50, -50, 0), rectTransform.sizeDelta);
	}

	//#설명#	자기 자신을 닫는다.
	public void UIWindowClose()
	{
		UIWindowManager.Instance.modalBackground.SetActive(false);
		Destroy(this.gameObject);
	}

	//#설명#	형제 객체들을 전부 닫아버린다.
	public void UIWindowSiblingAllClose()
	{
		UIWindowManager.Instance.UIWindowChildAllClose(transform.parent);
	}

	//#설명#	해당 UIWindow가 클릭되었을때 가장 앞으로 끌고오는 역할
	public void OnPointerClick(PointerEventData eventData)
	{
		BringToFront();
	}

	//#설명#	해당 UIWindow를 가장 앞으로 끌고오는 역할
	public void BringToFront()
	{
		rectTransform.SetAsLastSibling();
	}

	//#설명#	이벤트 호출기
	public void EventStarter(int eventNumber)
	{
		windowEvents[eventNumber]?.Invoke();
	}
}
