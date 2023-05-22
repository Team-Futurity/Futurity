using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartsSlotSettingButton : MonoBehaviour
{
	[Header ("파츠 데이터를 UI에 출력하거나, 저장소에 파츠 데이터를 전달하는 스크립트")]
	[Space (15)]


	[SerializeField]
	[Tooltip("파츠 슬롯의 넘버")]
	private int partsSlotNum;

	[SerializeField]
	[Tooltip("파츠의 정보가 담긴 스크립터블 오브젝트")]
	private ItemUIData partsData;

	[SerializeField]
	[Tooltip("파츠 이름을 출력할 TextMeshProUGUI 오브젝트")]
	private TextMeshProUGUI partsNameText;

	[SerializeField]
	[Tooltip("파츠 설명을 출력할 TextMeshProUGUI 오브젝트")]
	private TextMeshProUGUI partsMenualText;

	[SerializeField]
	[Tooltip("파츠 이미지를 출력할 ImageUI 오브젝트")]
	private Image partsSpriteWriter;

	private void Start()
	{
		if (!partsData && PartsRepositoryManager.Instance.GetRepositoryItemUIData(partsSlotNum) != null)
		{
			partsData = PartsRepositoryManager.Instance.GetRepositoryItemUIData(partsSlotNum);
		}

		if (partsData)
		{
			partsSpriteWriter.sprite = partsData.itemSprite;
			partsNameText.text = partsData.itemName;
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		//#설명#	선택시 파츠 설명 출력
		partsMenualText.text = partsData.itemDescription;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		//#설명#	선택 해제시 파츠 설명 제거
		partsMenualText.text = "";
	}


	public void SetRepositoryCurrentPartsData()
	{
		PartsRepositoryManager.Instance.SetItemUIData(partsSlotNum);
	}
}
