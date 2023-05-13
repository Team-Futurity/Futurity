using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
public class UIWindowController : MonoBehaviour, IPointerClickHandler
{
	public GameObject parentObject;
	public GameObject parentCanvas;
	[SerializeField]
	private RectTransform rectTransform; 
	
	//#����#	Ÿ UIWindow���� �տ� ���� ������ ���� UI
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

	//#����#	�ڱ� �ڽ��� �ݴ´�.
	public void UIWindowClose()
	{
		Destroy(modalBackground);
		Destroy(this.gameObject);
	}

	//#����#	���� ��ü���� ���� �ݾƹ�����.
	public void UIWindowSiblingAllClose()
	{
		UIManager.Instance.UIWindowChildAllClose(transform.parent);
	}

	//#����#	Ŭ���� UIWindow�� ���� ������ ������� ����
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
