using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PartsSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[SerializeField]
	[Tooltip("파츠 저장소 위치 (보통 Player가 가지고있음)")]
	private PartsRepositoryManager partsRepositoryManager;

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

	[SerializeField]
	[Tooltip("해당 버튼의 번호")]
	private int buttonNum;



	private void Start()
	{
		partsRepositoryManager = GameObject.Find("Player").GetComponent<PartsRepositoryManager>();

		itemUiData = partsRepositoryManager.GetEnemyData(buttonNum);

		if (itemUiData != null)
		{
			partsSpriteWriter.sprite = itemUiData.ItemSprite;
			partsNameText.text = itemUiData.ItemName;
		} 
		else
		{
			partsSpriteWriter.sprite = null;
			partsNameText.text = null;
		}
	}

	public void SetPartsData(ItemUIData newItemUiData)
	{
		itemUiData = newItemUiData;

		partsSpriteWriter.sprite = itemUiData.ItemSprite;
		partsNameText.text = itemUiData.ItemName;
	}

	public void OnSelect(BaseEventData eventData)
	{
		//#설명#	선택시 파츠 설명 출력
		partsMenualText.text = itemUiData.ItemDescription;
	}

	// 선택 해제시
	public void OnDeselect(BaseEventData eventData)
	{
		//#설명#	선택 해제시 파츠 설명 제거
		partsMenualText.text = "";
	}

	public void partsDataSelect()
	{
		// 파츠 데이터 저장
		partsRepositoryManager.SetCurrentPartsData(itemUiData);
	}
}
