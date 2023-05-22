using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PartsSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{

	[SerializeField]
	[Tooltip("������ ������ ��� ��ũ���ͺ� ������Ʈ")]
	private ItemUIData ItemUIData;

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
		ItemUIData = PartsRepositoryManager.Instance.GetEnemyData(buttonNum);

		if (ItemUIData != null)
		{
			partsSpriteWriter.sprite = ItemUIData.itemSprite;
			partsNameText.text = ItemUIData.itemName;
		}
		else
		{
			partsSpriteWriter.sprite = null;
			partsNameText.text = null;
		}
	}

	public void SetPartsData(ItemUIData newPartsData)
	{
		ItemUIData = newPartsData;

		partsSpriteWriter.sprite = ItemUIData.itemSprite;
		partsNameText.text = ItemUIData.itemName;
	}

	public void OnSelect(BaseEventData eventData)
	{
		//#����#	���ý� ���� ���� ���

		if (ItemUIData != null)
		{
			partsMenualText.text = ItemUIData.itemDescription;
		}
	}

	// ���� ������
	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	���� ������ ���� ���� ����

			partsMenualText.text = "";
	}

	public void ItemUIDataSelect()
	{
		// ���� ������ ����
		PartsRepositoryManager.Instance.SetCurrentItemUIData(ItemUIData);
	}
}

