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
	bool isGaugeMax = true;

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
		float gaugeBarValue = comboGuageBarController.GetGaugeFillAmount();


		if (gaugeBarValue >= 1f)
		{
			isGaugeMax = false;
		}
		else if (gaugeBarValue <= 0f)
		{
			isGaugeMax = true;
		}

		if (isGaugeMax)
		{
			comboGuageBarController.SetGaugeFillAmount(gaugeBarValue + 0.05f);
			hpGuageBarController.SetGaugeFillAmount(gaugeBarValue + 0.05f);
		}
		else
		{
			comboGuageBarController.SetGaugeFillAmount(gaugeBarValue - 0.05f);
			hpGuageBarController.SetGaugeFillAmount(gaugeBarValue + 0.05f);
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
