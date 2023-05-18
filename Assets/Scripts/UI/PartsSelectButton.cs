using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PartsSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
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
	private PartsRepositoryContorller partsSettingContorller;

	private void Start()
	{
		partsSpriteWriter.sprite = partsData.partsSprite;
		partsNameText.text = partsData.partsName;

		//#���濹��#	�ش�κ� find���� �ʵ��� �� ��
		partsSettingContorller = GameObject.Find("Player").GetComponent<PartsRepositoryContorller>();
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

	public void partsDataSelect()
	{
		// ���� ������ ����
		partsSettingContorller.SelectedPartsData(partsData);
	}
}
