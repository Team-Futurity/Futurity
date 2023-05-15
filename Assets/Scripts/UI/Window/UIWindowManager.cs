using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWindowManager : Singleton<UIWindowManager>
{
	public GameObject modalBackground;
	private GameObject parentCanvas;
	private Transform topCanvasTransform;

	private void Start()
	{
		if (!modalBackground)
		{
			modalBackground = new GameObject("modalBackground");
			modalBackground.AddComponent<CanvasRenderer>();
			Image image = modalBackground.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.5f);
		}


		topCanvasTransform = FindTopCanvas().transform;

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	//#����#	Scene�� �ε�� ������ topCanvasTransform�� �ٽ� �����ϱ� ���� FindTopCanvas�� ȣ���Ѵ�.
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		topCanvasTransform = FindTopCanvas().transform;
	}

	//#����#	���� ���� Canvas�� ã�Ƽ� topCanvasTransform�� �Ҵ���
	private GameObject FindTopCanvas()
	{
		Canvas[] allCanvases = FindObjectsOfType<Canvas>();

		Canvas topCanvas = null;
		foreach (Canvas canvas in allCanvases)
		{
			if (topCanvas == null || canvas.sortingOrder > topCanvas.sortingOrder)
			{
				topCanvas = canvas;
			}
		}
		
		if (topCanvas != null)
        {
            Debug.Log("Top Canvas: " + topCanvas.name);
			return topCanvas.gameObject;
		}
        else
        {
            Debug.Log("No Canvas found.");
			return null;
		}
	}

	public GameObject CreateModalBackground(GameObject uiWindow)
	{
		parentCanvas = FindParentCanvas(uiWindow);
		modalBackground.transform.parent = parentCanvas.transform;

		RectTransform modalRectTransform = modalBackground.GetComponent<RectTransform>();
		modalRectTransform.anchorMin = Vector2.zero;
		modalRectTransform.anchorMax = Vector2.one;
		modalRectTransform.offsetMin = Vector2.zero;
		modalRectTransform.offsetMax = Vector2.zero;
		modalRectTransform.SetAsLastSibling();

		modalRectTransform.SetSiblingIndex(modalRectTransform.GetSiblingIndex() - 1);

		modalBackground.SetActive(true);

		return modalBackground;
	}

	//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�
	public GameObject UIWindowOpen(GameObject OpenUiWindowObject, Transform uiParent, Vector2 instancePosition, Vector2 windowScale)
	{
		GameObject newUI = Instantiate(OpenUiWindowObject, uiParent);
		if (!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;
		rectTransform.localScale = windowScale;
		newUI.GetComponent<UIWindowController>().parentObject = uiParent.gameObject;

		return newUI;
	}

	//#����#	UI â�� �ν��Ͻ�ȭ�ϵ�, �θ� ���� ���� Canvas�� �����Ѵ�
	public GameObject UIWindowTopOpen(GameObject OpenUiWindowObject, Vector2 windowPosition, Vector2 windowScale)
	{
		GameObject newUI = Instantiate(OpenUiWindowObject, topCanvasTransform);
		if (!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = windowPosition;
		rectTransform.localScale = windowScale;
		newUI.GetComponent<UIWindowController>().parentObject = topCanvasTransform.gameObject;

		return newUI;
	}

	//#����#	��� �ڽ� UIâ�� �ݴ� �Լ�
	public void UIWindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if (child.CompareTag("UIWindow"))
				child.gameObject.GetComponent<UIWindowController>().UIWindowClose();
		}
	}


	//#����#	childObject�� �θ� Ŭ������ ���� ����� Canvas�� ã�� �Լ�
	public GameObject FindParentCanvas(GameObject childObject)
	{
		Transform parentTransform = childObject.transform;

		while (parentTransform != null)
		{
			if (parentTransform.GetComponent<Canvas>() != null)
			{
				return parentTransform.gameObject;
			}
			parentTransform = parentTransform.parent;

			if (parentTransform == null)
			{
				return null;
			}
		}

		return null;
	}
}
