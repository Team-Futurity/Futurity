using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScrollingCredits : MonoBehaviour
{
	public float speed = 50f;
	public GameObject textPrefab;
	public GameObject endObject;
	public List<string> credits = new List<string>();
	public Transform startPoint;
	public Transform meddlePoint;
	public Transform endPoint;
	public UnityEvent OnCreditsEnd;
	public float cutOffPoint = 600f;
	public float minimumSpacing = 100f;
	public float endObjectDelayTime = 2f; // EndObject가 멈춘 후 OnCreditsEnd를 호출하기까지의 대기 시간
	private List<GameObject> textObjects = new List<GameObject>();
	[SerializeField]
	private bool allTextsProcessed = false;
	private bool endObjectReachedMeddlePoint = false; // endObject가 중간 지점에 도달했는지 확인

	void Start()
	{
		endObject.SetActive(false);
		allTextsProcessed = false;
		CreateCredits();

		transform.parent = WindowManager.Instance.topCanvasTransform;
	}

	void Update()
	{
		GameObject lastText = null;

		for (int i = 0; i < textObjects.Count; i++)
		{
			GameObject currentText = textObjects[i];
			if (i > 0)
			{
				GameObject previousText = textObjects[i - 1];
				if (Vector3.Distance(previousText.transform.localPosition, currentText.transform.localPosition) < minimumSpacing)
				{
					continue;
				}
			}
			currentText.transform.Translate(Vector3.up * speed * Time.deltaTime);
			if (currentText.transform.localPosition.y > endPoint.localPosition.y * 2)
			{
				Destroy(currentText);
				textObjects.RemoveAt(i);
				i--;
			}
			else
			{
				lastText = currentText;
			}
		}

		if (lastText != null)
		{
			if (endObject.transform.localPosition.y < meddlePoint.localPosition.y * 2 && !endObjectReachedMeddlePoint)
			{
				endObject.transform.position = new Vector3(lastText.transform.position.x, lastText.transform.position.y - minimumSpacing, lastText.transform.position.z);
			}
			else if (!endObjectReachedMeddlePoint)
			{
				endObjectReachedMeddlePoint = true;
				StartCoroutine(WaitAndInvokeEndCredits(endObjectDelayTime));
			}

			if (!endObject.activeSelf)
			{
				endObject.SetActive(true);
			}
		}
		if (textObjects.Count == 0 && !allTextsProcessed)
		{
			Debug.Log("CreditEnd");
			allTextsProcessed = true;
		}
	}

	IEnumerator WaitAndInvokeEndCredits(float delay)
	{
		yield return new WaitForSeconds(delay);
		OnCreditsEnd.Invoke();
		Destroy(endObject);
	}

	void CreateCredits()
	{
		Vector3 currentPos = startPoint.position;
		foreach (string credit in credits)
		{
			GameObject textObject = Instantiate(textPrefab, currentPos, Quaternion.identity, startPoint);
			textObject.GetComponent<TextMeshProUGUI>().text = credit;
			textObjects.Add(textObject);
			currentPos -= new Vector3(0, minimumSpacing, 0);
		}
	}
}
