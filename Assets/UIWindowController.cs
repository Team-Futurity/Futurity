using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//#설명#	해당 스크립트는 UIWindow가 가지고 있는 핵심 스크립트로, UIWindow의 전반적인 행동 양식을 지니고 있습니다.
public class UIWindowController : MonoBehaviour, IPointerClickHandler
{
	public GameObject parentObject;
	public GameObject parentCanvas;
	[SerializeField]
	private RectTransform rectTransform; 
	
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
			UIWindowLock(true);
		}
	}

	public void UIWindowLock(bool isLockOnOff)
	{
		if (isLockOnOff)
		{
			if(!modalBackground)
			{
				modalBackground = new GameObject("modalBackground");
				modalBackground.AddComponent<CanvasRenderer>();
				Image image = modalBackground.AddComponent<Image>();
				image.color = new Color(0, 0, 0, 0.5f);
			}

			modalBackground = Instantiate(modalBackground, parentObject.transform);

			modalBackground.SetActive(true);

			RectTransform modalRectTransform = modalBackground.GetComponent<RectTransform>();

			modalRectTransform.anchorMin = Vector2.zero;
			modalRectTransform.anchorMax = Vector2.one;
			modalRectTransform.offsetMin = Vector2.zero;
			modalRectTransform.offsetMax = Vector2.zero;

			BringToFront();
		}
		else
		{
			modalBackground.SetActive(false);
			BringToFront();
		}
	}

	

	public void UIWindowNewOpen(GameObject OpenUIWindowObject)
    {
		UIManager.Instance.UIWindowOpen(OpenUIWindowObject, transform.parent, rectTransform.localPosition + new Vector3(50, -50, 0));
    }

	//#설명#	자기 자신을 닫는다.
	public void UIWindowClose()
	{
		Destroy(modalBackground);
		Destroy(this.gameObject);
	}

	//#설명#	형제 객체들을 전부 닫아버린다.
	public void UIWindowSiblingAllClose()
	{
		UIManager.Instance.UIWindowChildAllClose(transform.parent);
	}

	//#설명#	클릭된 UIWindow를 가장 앞으로 끌고오는 역할
	public void OnPointerClick(PointerEventData eventData)
	{
		BringToFront();
	}
	public void BringToFront()
	{
		rectTransform.SetAsLastSibling();
	}

	public GameObject FindParentCanvas()
	{
		GameObject canvasObject = null;
		Transform parentTransform = transform;

		while (parentTransform != null)
		{
			if (parentTransform.GetComponent<Canvas>() != null)
			{
				break;
			}
			parentTransform = parentTransform.parent;
		}

		return canvasObject;
	}
}
