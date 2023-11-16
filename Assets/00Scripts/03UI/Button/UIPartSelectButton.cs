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

	public Image activeTypeImage;
	public Image passiveTypeImage;
	

	protected override void ActiveFunc()
	{
		onSelected?.Invoke(partCode, buttonIndex);
	}

	public override void Init()
	{
		if(PartIconImage != null)
			PartIconImage.color = Define.noneSelectColor;
		if (PartNameImage != null)
			PartNameImage.color = Define.noneSelectColor;
		if (CoreInfoText != null)
			CoreInfoText.color = SubInfoText.color = Define.noneSelectColor;
		if (partTypeImage != null)
			partTypeImage.color = Define.noneSelectColor;
		if(infoText != null)
			infoText.color = Define.noneSelectColor;
		if(activeTypeImage != null)
			activeTypeImage.color = Define.noneSelectColor;
		if (passiveTypeImage != null)
		{
			passiveTypeImage.color = Define.noneSelectColor;
			Debug.Log("CALL");
			
			Debug.Log(Define.noneSelectColor);
		}
	}

	public override void SelectActive(bool isOn)
	{
		if(isOn)
		{
			if (PartIconImage != null)
				PartIconImage.color = Define.selectColor;
			if (PartNameImage != null)
				PartNameImage.color = Define.selectColor;
			if (CoreInfoText != null)
				CoreInfoText.color = SubInfoText.color = Define.selectColor;
			if (partTypeImage != null)
				partTypeImage.color = Define.selectColor;
			if (infoText != null)
				infoText.color = Define.selectColor;
			if(activeTypeImage != null)
				activeTypeImage.color = Define.selectColor;
			if(passiveTypeImage != null)
				passiveTypeImage.color = Define.selectColor;
		}
		else
		{
			if(PartIconImage != null)
				PartIconImage.color = Define.noneSelectColor;
			if (PartNameImage != null)
				PartNameImage.color = Define.noneSelectColor;
			if (CoreInfoText != null)
				CoreInfoText.color = SubInfoText.color = Define.noneSelectColor;
			if (partTypeImage != null)
				partTypeImage.color = Define.noneSelectColor;
			if(infoText != null)
				infoText.color = Define.noneSelectColor;
			if(activeTypeImage != null)
				activeTypeImage.color = Define.noneSelectColor;
			if(passiveTypeImage != null)
				passiveTypeImage.color = Define.noneSelectColor;
		}
	}

	public void SetButtonData(int code)
	{
		PartIconImage.enabled = PartNameImage.enabled = true;

		if (code == 2201 || code == 2202)
		{
			partTypeImage.sprite = activeImage;
			if(activeObj != null)
				activeObj.SetActive(true);
			if(passiveObj != null)
				passiveObj.SetActive(false);
			
			var data = LoadPartData(code);
			
			if(infoText != null)
				infoText.text = data.coreInfoText;
			ChangeResource(data, true);
		}
		else
		{
			partTypeImage.sprite = passiveImage;
			if(passiveObj != null)
				passiveObj.SetActive(true);
			
			if(activeObj != null)
				activeObj.SetActive(false);
			
			ChangeResource(LoadPartData(code));
		}
		
		partCode = code;
	}

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
