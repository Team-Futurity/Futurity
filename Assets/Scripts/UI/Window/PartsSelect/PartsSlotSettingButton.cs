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
	private ItemUIData itemUiData;

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
		if (!itemUiData && PartsRepositoryManager.Instance.GetRepositoryPartsData(partsSlotNum) != null)
		{
			itemUiData = PartsRepositoryManager.Instance.GetRepositoryPartsData(partsSlotNum);
		}

		if (itemUiData)
		{
			partsSpriteWriter.sprite = itemUiData.ItemSprite;
			partsNameText.text = itemUiData.ItemName;
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		//#설명#	선택시 파츠 설명 출력
		if (itemUiData != null)
		{
			partsMenualText.text = itemUiData.ItemDescription;
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		//#설명#	선택 해제시 파츠 설명 제거
		partsMenualText.text = "";
	}


	///<summary>
	/// 선택한 해당 스크립트의 파츠 번호 데이터를 저장소에 전달합니다.
	///</summary>
	public void SetRepositoryCurrentPartsData()
	{
		PartsRepositoryManager.Instance.SetPartsData(partsSlotNum);
	}
}
