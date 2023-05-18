using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrapUIController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textMeshProUGUI;

	[SerializeField]
	private int scrapValue = 10;
	[SerializeField]
	private float typingTime = 1f;
	[SerializeField]
	private int typingInterval = 10;


	public int GetScrapValue()
	{
		return scrapValue;
	}
	public void SetScrapValue(int setScrapValue)
	{
		StartCoroutine(ScrapUplorder(setScrapValue));
	}

	IEnumerator ScrapUplorder(int setScrapValue)
	{
		float plusScrap = (setScrapValue - scrapValue) / typingInterval;

		for (int i = 1; i < typingInterval; i++)
		{
			scrapValue += (int)plusScrap;
			textMeshProUGUI.text = $"{scrapValue}";

			yield return new WaitForSeconds(typingTime / (float)typingInterval);
		}

		Debug.Log($"scrapValue : {scrapValue}");
		scrapValue = setScrapValue;
		textMeshProUGUI.text = $"{scrapValue}";
	}
}
