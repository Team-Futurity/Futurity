using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class UIPartSelectButton : UIButton
{
	[SerializeField]
	private int partCode;

	#region Resource
	
	[field: SerializeField]
	public Image PartIconImage { get; private set; }

	[field: SerializeField]
	public Image PartNameImage { get; private set; }

	[field: SerializeField]
	public TMP_Text CoreInfoText { get; private set; }

	[field: SerializeField]
	public TMP_Text SubInfoText { get; private set; }
	
	#endregion

	public Image partTypeImage;
	public Sprite passiveImage;
	public Sprite activeImage;

	public GameObject passiveObj;
	public GameObject activeObj;

	public TMP_Text infoText;

	[HideInInspector]
	public UnityEvent<int, int> onSelected;
	
	[SerializeField]
	private int buttonIndex = 0;

	protected override void ActiveFunc()
	{
		onSelected?.Invoke(partCode, buttonIndex);
	}

	public void SetButtonData(int code)
	{
		PartIconImage.enabled = PartNameImage.enabled = true;

		if (code == 2201 || code == 2202)
		{
			partTypeImage.sprite = activeImage;
			activeObj.SetActive(true);
			passiveObj.SetActive(false);
			
			var data = LoadPartData(code);
			infoText.text = data.coreInfoText;
			ChangeResource(data, true);
		}
		else
		{
			ChangeResource(LoadPartData(code));

			if (partTypeImage == null || passiveObj == null || activeObj == null)
				return;
			
			partTypeImage.sprite = passiveImage;
			passiveObj.SetActive(true);
			activeObj.SetActive(false);
		}
		
		partCode = code;
	}

	// Addressable¿ª ≈Î«— Data Load
	private UIPassiveSelectData LoadPartData(int code)
	{
		UIPassiveSelectData data = Addressables.LoadAssetAsync<UIPassiveSelectData>(code.ToString()).WaitForCompletion();

		return data;
	}

	private void ChangeResource(UIPassiveSelectData selectData, bool isActive = false)
	{
		PartIconImage.sprite = selectData.partIconSpr;
		PartNameImage.sprite = selectData.partNameSpr;

		if (!isActive)
		{
			CoreInfoText.text = selectData.coreInfoText;
			SubInfoText.text = selectData.subInfoText;
		}
	}

	public void InitResource(bool isActive = false)
	{
		PartIconImage.enabled = PartNameImage.enabled = false;

		if (!isActive)
		{
			CoreInfoText.text = SubInfoText.text = "";
		}
	}
}
