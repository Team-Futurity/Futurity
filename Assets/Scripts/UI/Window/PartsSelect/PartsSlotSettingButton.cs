using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartsSlotSettingButton : MonoBehaviour
{
	[Header ("���� �����͸� UI�� ����ϰų�, ����ҿ� ���� �����͸� �����ϴ� ��ũ��Ʈ")]
	[Space (15)]


	[SerializeField]
	[Tooltip("���� ������ �ѹ�")]
	private int partsSlotNum;

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
		//#����#	���ý� ���� ���� ���
		if (itemUiData != null)
		{
			partsMenualText.text = itemUiData.ItemDescription;
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	���� ������ ���� ���� ����
		partsMenualText.text = "";
	}


	///<summary>
	/// ������ �ش� ��ũ��Ʈ�� ���� ��ȣ �����͸� ����ҿ� �����մϴ�.
	///</summary>
	public void SetRepositoryCurrentPartsData()
	{
		PartsRepositoryManager.Instance.SetPartsData(partsSlotNum);
	}
}
