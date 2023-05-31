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
	[Tooltip("���� �̹����� ����� ImageUI sprite")]
	private GameObject partsSpriteObject;
	private Image partsSprite;
	private Color deselectColor = new Color(0.5f, 0.5f, 0.5f);

	[SerializeField]
	[Tooltip("�ش� ��ư�� ��ȣ")]
	private int buttonNum;


	/// <summary>
	/// �ش� Button ������ �� �̹����� �Ҵ��ϰ�, 0��° ��ư�� �ƴ϶�� color���� �������� �����.
	/// </summary>
	private void Start()
	{ 
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
			partsMenualText.text = "";
		}
	}

	///<summary>
	/// ���ο� ������ �����͸� �Ҵ��Ѵ�.
	///</summary>
	/// <param name="newItemUIData">���Ӱ� �Ҵ��� ItemUIData</param>
	public void SetItemUIData(ItemUIData newItemUIData)
	{
		itemUiData = newItemUIData;

		partsSprite.sprite = itemUiData.ItemSprite;
	}

	#region ButtonSelect

	///<summary>
	/// "����"�� ���� ���� ���
	///</summary>
	public void OnSelect(BaseEventData eventData)
	{
		if (itemUiData is not null)
		{
			partsMenualText.text = itemUiData.ItemDescription;
			partsNameText.text = itemUiData.ItemName;
			partsSprite.color = Color.white;
		}
	}

	/// <summary>
	/// "���� ����"�� ���� ���� ����
	/// </summary>
	public void OnDeselect(BaseEventData eventData)
	{
		//#����#	"���� ����"�� ���� ���� ����
		partsMenualText.text = "";
		partsSprite.color = deselectColor;
	}
	#endregion
}
