using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartsSlotSettingButton : MonoBehaviour
{
	[SerializeField]
	[Tooltip("파츠 슬롯의 넘버")]
	private int partsSlotNum;

	[SerializeField]
	[Tooltip("파츠의 정보가 담긴 스크립터블 오브젝트")]
	private PartsData partsData;

	[SerializeField]
	[Tooltip("파츠 이름을 출력할 TextMeshProUGUI 오브젝트")]
	private TextMeshProUGUI partsNameText;

	[SerializeField]
	[Tooltip("파츠 설명을 출력할 TextMeshProUGUI 오브젝트")]
	private TextMeshProUGUI partsMenualText;

	[SerializeField]
	[Tooltip("파츠 이미지를 출력할 ImageUI 오브젝트")]
	private Image partsSpriteWriter;

	[SerializeField]
	[Tooltip("선택된 파츠를 저장하는 PartsSettingController 오브젝트")]
	private PartsRepositoryContorller partsRepositoryContorller;

	private void Start()
	{
		//#변경예정#	해당부분 find쓰지 않도록 할 것
		partsRepositoryContorller = GameObject.Find("Player").GetComponent<PartsRepositoryContorller>();

		if(!partsData)
		{
			partsData = partsRepositoryContorller.GetRepositoryPartsData(partsSlotNum);
		}

		if (partsData)
		{
			partsSpriteWriter.sprite = partsData.partsSprite;
			partsNameText.text = partsData.partsName;
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		//#설명#	선택시 파츠 설명 출력
		partsMenualText.text = partsData.partsMenual;
	}

	// 선택 해제시
	public void OnDeselect(BaseEventData eventData)
	{
		//#설명#	선택 해제시 파츠 설명 제거
		partsMenualText.text = "";
	}


	public void SetRepositoryCurrentPartsData()
	{
		partsRepositoryContorller.SettingPartsData(partsSlotNum);
	}
}
