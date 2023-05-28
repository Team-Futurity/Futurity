using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class TempTestManager : MonoBehaviour
{
	[SerializeField]
	UnityEvent startEvent;

	[SerializeField]
	List<GaugeBarController> guageBarControllers;

	float targetGaugeValue;
	bool isValueReached = true;

	[SerializeField]
	NumberImageLoader numberImageLoader;

	[SerializeField]
	WindowOpenController openController;
	[SerializeField]
	CharacterDialogController characterDialogController;
	bool isAdWard = true;

	[SerializeField]
	ScrapUIController scrapUIController;
	int scrapValue = 35;
	bool isMaxScrap = false;

	void Start()
	{
		characterDialogController = openController.WindowActiveOpen().GetComponent<CharacterDialogController>();

		TextValueSetter();
		scrapUIController.SetScrapValue(scrapValue);
		StartCoroutine(NumberImageValueChanger());
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
		float gaugeBarValue = guageBarControllers[0].gaugeBar.fillAmount;

		if (Mathf.Abs(gaugeBarValue - targetGaugeValue) <= 0.01f)
		{
			isValueReached = true;
		}

		if (isValueReached)
		{
			targetGaugeValue = Random.Range(0f, 1f);

			for (int i = 0; i < guageBarControllers.Count; i++)
			{
				guageBarControllers[i].SetGaugeFillAmount(targetGaugeValue);
				isValueReached = false;

				Debug.Log($"{guageBarControllers[i].name} : {guageBarControllers[0].GetGaugeIntegerPercent()}");
			}
		}
	}


	IEnumerator NumberImageValueChanger()
	{
		int comboDelayCount = 0;
		while (true) // 무한 반복
		{
			int randValue = Random.Range(0, 999);
			numberImageLoader.SetNumber(randValue);

			if (comboDelayCount < 5)
			{
				yield return new WaitForSeconds(1f);
				comboDelayCount++;
			}
			else
			{
				yield return new WaitForSeconds(8f);
				comboDelayCount = 0;
			}
		}
	}

	void TextValueSetter()
	{
		characterDialogController.SetCharactorText($"동해물과 백두산이 마르고 닳도록\r\n하느님이 보우하사 우리나라 만세\r\n무궁화 삼천리 화려강산 대한사람 대한으로 길이\r\n보전하세. 남산 위에 저 소나무 철갑을 두른 듯\r\n바람서리 불변함은 우리기상 일세\r\n무궁화 삼천리 화려강산 대한사람 대한으로 길이보전하세 \r\n");
	}

	void TextValueChangerReturn()
	{
		if (characterDialogController.GetTextEnd())
		{
			if (isAdWard)
			{
				characterDialogController.SetTypingDelay(0.001f);
				characterDialogController.SetCharactorText($"에드워드... 오니짱......\n에드워드... 오니짱......\n에드워드... 오니짱......\n에드워드... 오니짱......\n에드워드... 오니짱......\n에드워드... 오니짱......\n에드워드... 오니짱......\n");
				isAdWard = false;
			} else
			{
				characterDialogController.SetTypingDelay(0.05f);
				characterDialogController.SetCharactorText($"동해물과 백두산이 마르고 닳도록\r\n하느님이 보우하사 우리나라 만세\r\n무궁화 삼천리 화려강산 대한사람 대한으로 길이\r\n보전하세. 남산 위에 저 소나무 철갑을 두른 듯\r\n바람서리 불변함은 우리기상 일세\r\n무궁화 삼천리 화려강산 대한사람 대한으로 길이보전하세 \r\n");
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
