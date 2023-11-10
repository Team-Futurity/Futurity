using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIDialogNpcNameViewer : UIDialogFeatureBase
{
	[SerializeField, Header("NPC 이름 이미지")]
	private Image nameImage;

	[SerializeField]
	private GameObject sari;
	[SerializeField]
	private GameObject mirae;
	[SerializeField]
	private GameObject boss;

	protected override void UpdateFeature()
	{
		nameImage.sprite = LoadNPCNameSprite(dialogData.talker_Eng);

		switch(dialogData.talker_Eng)
		{
			case "Sari":
				sari.SetActive(true);
				mirae.SetActive(false);
				boss.SetActive(false);
				break;

			case "Mirae":
				sari.SetActive(false);
				mirae.SetActive(true);
				boss.SetActive(false);
				break;

			case "Boss":
				sari.SetActive(false);
				mirae.SetActive(false);
				boss.SetActive(true);
				break;
		}
	}

	private Sprite LoadNPCNameSprite(string key)
	{
		var spr = Addressables.LoadAssetAsync<Sprite>(key).WaitForCompletion();
		return spr;
	}
}
