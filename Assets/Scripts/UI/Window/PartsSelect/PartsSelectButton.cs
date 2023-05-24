using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PartsSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{

	[SerializeField]
	[Tooltip("������ ������ ��� ��ũ���ͺ� ������Ʈ")]
	private ItemUIData itemUiData;

	[SerializeField]
	[Tooltip("���� �̸��� ����� TextMeshProUGUI ������Ʈ")]
	private TextMeshProUGUI partsNameText;

	[SerializeField]
	[Tooltip("���� ������ ����� TextMeshProUGUI ������Ʈ")]
	private TextMeshProUGUI partsMenualText;

	[SerializeField]
	[Tooltip("���� �̹����� ����� ImageUI ������Ʈ")]
	private Image partsSpriteWriter;

	[SerializeField]
	[Tooltip("�ش� ��ư�� ��ȣ")]
	private int buttonNum;



	private void Start()
	{
		itemUiData = PartsRepositoryManager.Instance.GetEnemyData(buttonNum);

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
		//#����#	���ý� ���� ���� ���
		partsMenualText.text = itemUiData.ItemDescription;
	}

	// ���� ������
	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	���� ������ ���� ���� ����
		partsMenualText.text = "";
	}

	public void partsDataSelect()
	{
		// ���� ������ ����
		PartsRepositoryManager.Instance.SetCurrentPartsData(itemUiData);
	}
}
