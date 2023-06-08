using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PartsSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
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
	private Image partsImage;
	private Color deselectColor = new Color(0.5f, 0.5f, 0.5f);

	[SerializeField]
	[Tooltip("해당 버튼의 번호")]
	private int buttonNum;


	/// <summary>
	/// 해당 Button 생성시 각 이미지를 할당하고, 0번째 버튼이 아니라면 color값을 절반으로 낮춘다.
	/// </summary>
	private void Start()
	{
		partsImage = partsSpriteObject.GetComponent<Image>();

		if (buttonNum != 0)
		{
			partsImage.color = deselectColor;
		} else
		{
			partsImage.color = Color.white;
		}

		if (itemUiData is not null)
		{
			partsImage.sprite = itemUiData.ItemSprite;
			partsMenualText.text = itemUiData.ItemDescription;

		} 
		else
		{
			partsImage.sprite = null;
			partsMenualText.text = "";
		}
	}

	///<summary>
	/// 세로운 아이템 데이터를 할당한다.
	///</summary>
	/// <param name="newItemUIData">세롭게 할당할 ItemUIData</param>
	public void SetItemUIData(ItemUIData newItemUIData)
	{
		itemUiData = newItemUIData;

		partsImage.sprite = itemUiData.ItemSprite; 
	}


	/// <summary>
	/// 세로운 아이템 데이터를 전달한다.
	/// </summary>
	/// <returns>해당 버튼의 아이템 데이터</returns>
	public ItemUIData GetItemUIData()
	{
		return itemUiData;
	}

	#region ButtonSelect

	///<summary>
	/// "선택"시 파츠 설명 출력
	///</summary>
	public void OnSelect(BaseEventData eventData)
	{
		if (partsImage is not null)
		{
			partsImage.color = Color.white;
		}
		if (itemUiData is not null)
		{
			partsMenualText.text = itemUiData.ItemDescription;
			partsNameText.text = itemUiData.ItemName;
		}
	}

	/// <summary>
	/// "선택 해제"시 파츠 설명 제거
	/// </summary>
	public void OnDeselect(BaseEventData eventData)
	{
		//#설명#	"선택 해제"시 파츠 설명 제거
		partsMenualText.text = "";
		if (partsImage is not null)
		{
			partsImage.color = deselectColor;
		}
	}
	#endregion
}
