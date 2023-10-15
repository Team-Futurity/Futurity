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

	[HideInInspector]
	public UnityEvent<int, int> onSelected;
	
	[SerializeField]
	private int buttonIndex = 0;

	private void Awake()
	{
		InitResource();
	}

	protected override void ActiveFunc()
	{
		onSelected?.Invoke(partCode, buttonIndex);
	}

	public void SetButtonData(int code)
	{
		PartIconImage.enabled = PartNameImage.enabled = true;
		ChangeResource(LoadPartData(code));
	}

	// Addressable¿ª ≈Î«— Data Load
	private UIPassiveSelectData LoadPartData(int code)
	{
		UIPassiveSelectData data = Addressables.LoadAssetAsync<UIPassiveSelectData>(code.ToString()).WaitForCompletion();
		return data;
	}

	private void ChangeResource(UIPassiveSelectData selectData)
	{
		PartIconImage.sprite = selectData.partIconSpr;
		PartNameImage.sprite = selectData.partNameSpr;
		CoreInfoText.text = selectData.coreInfoText;
		SubInfoText.text = selectData.subInfoText;
	}

	private void InitResource()
	{
		PartIconImage.enabled = PartNameImage.enabled = false;
		CoreInfoText.text = SubInfoText.text = "";
	}
}
