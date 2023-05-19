using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TempTestManager : MonoBehaviour
{
	[SerializeField]
	GaugeBarController comboGuageBarController;
	[SerializeField]
	GaugeBarController hpGuageBarController;
	float targetGaugeValue;
	bool isValueReached = true;

	[SerializeField]
	NumberImageLoader numberImageLoader;

	[SerializeField]
	CharacterDialogController characterDialogController;
	bool isAdWard = true;

	[SerializeField]
	ScrapUIController scrapUIController;
	int scrapValue = 35;
	bool isMaxScrap = false;

	void Start()
	{
		TextValueChanger();
		scrapUIController.SetScrapValue(scrapValue);
	}


    void Update()
	{
		ComboGuageBarValueChanger();
		NumberImageValueChanger();
		TextValueChangerReturn();
		ScrapValueChangerReturn();
	}

	void ComboGuageBarValueChanger()
	{
		float gaugeBarValue = comboGuageBarController.comboGaugeBar.fillAmount;

		if (Mathf.Abs(gaugeBarValue - targetGaugeValue) <= 0.01f)
		{
			isValueReached = true;
		}

		if (isValueReached)
		{
			targetGaugeValue = Random.Range(0f, 1f);
			comboGuageBarController.SetGaugeFillAmount(targetGaugeValue);
			hpGuageBarController.SetGaugeFillAmount(targetGaugeValue);
			isValueReached = false;
		}

		Debug.Log($"GetGaugeIntegerPercent : {comboGuageBarController.GetGaugeIntegerPercent()}");
		Debug.Log($"hpGuageBarController : {hpGuageBarController.GetGaugeIntegerPercent()}");
	}



	void NumberImageValueChanger()
	{
		int randValue = Random.Range(0, 999);

		numberImageLoader.SetNumber(randValue);
	}

	void TextValueChanger()
	{
		characterDialogController.SetCharactorText($"¿¡µå¿öµå... \n¿À´ÏÂ¯......");
	}

	void TextValueChangerReturn()
	{
		if (characterDialogController.GetTextEnd())
		{
			if (isAdWard)
			{
				characterDialogController.SetCharactorText($"ÀÕ¼î´Ï... \n¾Æ¼Òº¸......?");
				isAdWard = false;
			} else
			{
				characterDialogController.SetCharactorText($"¿¡µå¿öµå... \n¿À´ÏÂ¯......");
				isAdWard = true;
			}
		}
	}

	void ScrapValueChangerReturn()
	{
		if (scrapUIController.GetScrapValue() >= scrapValue)
		{
			if (!isMaxScrap)
			{
				isMaxScrap = true;
				scrapUIController.SetScrapValue(0);
			} else
			{
				isMaxScrap = false;
			}
		}
		else if (scrapUIController.GetScrapValue() <= 0)
		{
			if (isMaxScrap)
			{
				isMaxScrap = false;
				scrapUIController.SetScrapValue(scrapValue);
			}
			else
			{
				isMaxScrap = true;
			}
		}
	}
}
