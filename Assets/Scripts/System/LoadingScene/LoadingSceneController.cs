using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
	[Header("LoadingScene 동기 오브젝트")]
	[SerializeField]
	public Image loadingBar;
	[SerializeField]
	private GameObject menualObject;
	[SerializeField]
	private GameObject stageNameObject;

	[Space(10)]
	[Header("LoadingScene 하단 텍스트 목록")]
	[SerializeField]
	private List<string> menualText;


	void Start()
    {
		if (menualObject != null)
		{
			menualObject.GetComponent<TextMeshProUGUI>().text = menualText[UnityEngine.Random.Range(0, menualText.Count - 1)];
		}
    }

	public void SetStageNameObject(string chapterNum, string sceneName, string incidentName)
	{
		string chapterNumTemp;
		string sceneNameTemp;
		string incidentNameTemp;


		if (chapterNum == null)
		{
			chapterNumTemp = "챕터";
		}
		else
		{
			chapterNumTemp = chapterNum;
		}

		if (sceneName == null)
		{
			sceneNameTemp = "스테이지";
		}
		else
		{
			sceneNameTemp = sceneName;
		}

		if (incidentName == null)
		{
			incidentNameTemp = "사건";
		}
		else
		{
			incidentNameTemp = incidentName;
		}


		if (stageNameObject != null)
		{
			stageNameObject.GetComponent<TextMeshProUGUI>().text = $"{chapterNumTemp}    {sceneNameTemp}    {incidentNameTemp}";
		}
	}
}
