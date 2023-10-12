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

	[SerializeField]
	private bool isSelectMode = false;

	private int buttonIndex = 0;
	
	[field: SerializeField]
	public Image PartIconImage { get; private set; }

	[field: SerializeField]
	public Image PartNameImage { get; private set; }

	[field: SerializeField]
	public TMP_Text CoreInfoText { get; private set; }

	[field: SerializeField]
	public TMP_Text SubInfoText { get; private set; }

	[HideInInspector]
	public UnityEvent<int, int> onActive;

	protected override void ActiveFunc()
	{
		// 선택이 되었을 때
		onActive?.Invoke(partCode, buttonIndex);
	}

	public void SetButtonData(int code, int index)
	{
		ChangeResource(LoadPartData(code));
		SetIndex(index);
	}

	private void SetIndex(int index)
	{
		buttonIndex = index;
	}

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
}
