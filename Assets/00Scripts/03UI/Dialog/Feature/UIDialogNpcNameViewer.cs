using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIDialogNpcNameViewer : UIDialogFeatureBase
{
	[SerializeField, Header("NPC �̸� �̹���")]
	private Image nameImage;

	protected override void UpdateFeature()
	{
		nameImage.sprite = LoadNPCNameSprite(dialogData.talker_Eng);
	}

	private Sprite LoadNPCNameSprite(string key)
	{
		var spr = Addressables.LoadAssetAsync<Sprite>(key).WaitForCompletion();
		return spr;
	}
}
