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
	[Tooltip("�ش� ��ư�� ��ȣ")]
	private int buttonNum;



	private void Start()
	{
		partsData = PartsRepositoryManager.Instance.GetEnemyData(buttonNum);

		if (partsData != null)
		{
			partsSpriteWriter.sprite = partsData.partsSprite;
			partsNameText.text = partsData.partsName;
		} else
		{
			partsSpriteWriter.sprite = null;
			partsNameText.text = null;
		}
	}

	public void SetPartsData(PartsData newPartsData)
	{
		partsData = newPartsData;

		partsSpriteWriter.sprite = partsData.partsSprite;
		partsNameText.text = partsData.partsName;
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
		PartsRepositoryManager.Instance.SetCurrentPartsData(partsData);
	}
}
