using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PartsSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[SerializeField]
	[Tooltip("���� ����� ��ġ (���� Player�� ����������)")]
	private PartsRepositoryManager partsRepositoryManager;

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
		partsRepositoryManager = GameObject.Find("Player").GetComponent<PartsRepositoryManager>();

		itemUiData = partsRepositoryManager.GetEnemyData(buttonNum);

		if (itemUiData is not null)
		{
			partsSpriteWriter.sprite = itemUiData.ItemSprite;
			partsMenualText.text = itemUiData.ItemDescription;

		} 
		else
		{
			partsSpriteWriter.sprite = null;
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

		if (itemUiData is not null)
		{
			partsMenualText.text = itemUiData.ItemDescription;
			partsNameText.text = itemUiData.ItemName;
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	���� ������ ���� ���� ����
		partsMenualText.text = "";
	}

	public void PartsDataSelect()
	{
		//#����#	���� ������ ����
		partsRepositoryManager.SetCurrentPartsData(itemUiData);
	}
}
