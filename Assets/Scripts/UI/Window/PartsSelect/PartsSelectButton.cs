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
	[Tooltip("파츠 이미지를 출력할 ImageUI sprite")]
	private GameObject partsSpriteObject;
	private Image partsSprite;
	private Color deselectColor = new Color(0.5f, 0.5f, 0.5f);

	[SerializeField]
	[Tooltip("해당 버튼의 번호")]
	private int buttonNum;



	private void Start()
	{
		partsRepositoryManager = GameObject.Find("Player").GetComponent<PartsRepositoryManager>();

		itemUiData = partsRepositoryManager.GetEnemyData(buttonNum);

		if (itemUiData is not null)
		{
			partsSprite = partsSpriteObject.GetComponent<Image>();
			if (buttonNum != 0)
			{
				partsSprite.color = deselectColor;
			}

			partsSprite.sprite = itemUiData.ItemSprite;
			partsMenualText.text = itemUiData.ItemDescription;

		} 
		else
		{
			partsSprite.sprite = null;
		}
	}

	public void SetPartsData(ItemUIData newItemUiData)
	{
		itemUiData = newItemUiData;

		partsSprite.sprite = itemUiData.ItemSprite;
		partsNameText.text = itemUiData.ItemName;
	}

	public void OnSelect(BaseEventData eventData)
	{
		//#설명#	선택시 파츠 설명 출력

		if (itemUiData is not null)
		{
			partsMenualText.text = itemUiData.ItemDescription;
			partsNameText.text = itemUiData.ItemName;
			partsSprite.color = Color.white;
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		//#설명#	선택 해제시 파츠 설명 제거
		partsMenualText.text = "";
		partsSprite.color = deselectColor;
	}

	public void PartsDataSelect()
	{
		//#설명#	파츠 데이터 저장
		partsRepositoryManager.SetCurrentPartsData(itemUiData);
	}
}
