using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartsSlotSettingButton : MonoBehaviour
{
	[SerializeField]
	[Tooltip("���� ������ �ѹ�")]
	private int partsSlotNum;

	[SerializeField]
	[Tooltip("������ ������ ��� ��ũ���ͺ� ������Ʈ")]
	private PartsData partsData;

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
	[Tooltip("���õ� ������ �����ϴ� PartsSettingController ������Ʈ")]
	private PartsRepositoryContorller partsRepositoryContorller;

	private void Start()
	{
		//#���濹��#	�ش�κ� find���� �ʵ��� �� ��
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
		//#����#	���ý� ���� ���� ���
		partsMenualText.text = partsData.partsMenual;
	}

	// ���� ������
	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	���� ������ ���� ���� ����
		partsMenualText.text = "";
	}


	public void SetRepositoryCurrentPartsData()
	{
		partsRepositoryContorller.SettingPartsData(partsSlotNum);
	}
}
