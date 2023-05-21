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

	private void Start()
	{
		if (!partsData && PartsRepositoryManager.Instance.GetRepositoryPartsData(partsSlotNum) != null)
		{
			partsData = PartsRepositoryManager.Instance.GetRepositoryPartsData(partsSlotNum);
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

	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	���� ������ ���� ���� ����
		partsMenualText.text = "";
	}


	public void SetRepositoryCurrentPartsData()
	{
		PartsRepositoryManager.Instance.SetPartsData(partsSlotNum);
	}
}
