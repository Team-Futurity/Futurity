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

	//#설명#	Scene이 로드될 때마다 topCanvasTransform을 다시 설정하기 위해 FindTopCanvas를 호출한다.
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		topCanvasTransform = FindTopCanvas().transform;
	}

	//#설명#	가장 상위 Canvas를 찾아서 topCanvasTransform에 할당함
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

	//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수
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

	//#설명#	UI 창을 인스턴스화하되, 부모를 가장 상위 Canvas로 설정한다
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

	//#설명#	모든 자식 UI창을 닫는 함수
	public void UIWindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if (child.CompareTag("UIWindow"))
				child.gameObject.GetComponent<UIWindowController>().UIWindowClose();
		}
	}


	//#설명#	childObject의 부모 클래스중 가장 가까운 Canvas를 찾는 함수
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
