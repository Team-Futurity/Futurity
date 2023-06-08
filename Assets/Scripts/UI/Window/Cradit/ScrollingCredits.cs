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
	public Transform endPoint;
	public UnityEvent OnCreditsEnd;
	public float cutOffPoint = 600f;
	public float minimumSpacing = 100f;
	private List<GameObject> textObjects = new List<GameObject>();
	[SerializeField]
	private bool allTextsProcessed = false;

	void Start()
	{
		endObject.SetActive(false);
		allTextsProcessed = false;
		CreateCredits();
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

	void Update()
	{
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
		}
		if (textObjects.Count == 0 && !allTextsProcessed)
		{
			FDebug.Log("CraditEnd");
			allTextsProcessed = true;
			endObject.SetActive(true);
			OnCreditsEnd.Invoke();
		}
	}
}
