using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPartSelectButton : UIButton
{
	[field: SerializeField]
	public Image PartIconImage { get; private set; }

	[field: SerializeField]
	public Image PartNameImage { get; private set; }

	[field: SerializeField]
	public TMP_Text CoreInfoText { get; private set; }

	[field: SerializeField]
	public TMP_Text SubInfoText { get; private set; }

	protected override void ActiveFunc()
	{

	}

	public void ChangeResource(UIPassiveSelectData selectData)
	{
		PartIconImage.sprite = selectData.partIconSpr;
		PartNameImage.sprite = selectData.partNameSpr;
		CoreInfoText.text = selectData.coreInfoText;
		SubInfoText.text = selectData.subInfoText;
	}
}
