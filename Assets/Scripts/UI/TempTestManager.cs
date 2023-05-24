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
	[SerializeField]
	GaugeBarController bossHpGuageBarController;
	[SerializeField]
	GaugeBarController enemyGauageBarrController;

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
		float gaugeBarValue = comboGuageBarController.gaugeBar.fillAmount;

		if (Mathf.Abs(gaugeBarValue - targetGaugeValue) <= 0.01f)
		{
			isValueReached = true;
		}

		if (isValueReached)
		{
			targetGaugeValue = Random.Range(0f, 1f);
			comboGuageBarController.SetGaugeFillAmount(targetGaugeValue);
			hpGuageBarController.SetGaugeFillAmount(targetGaugeValue);
			bossHpGuageBarController.SetGaugeFillAmount(targetGaugeValue);
			enemyGauageBarrController.SetGaugeFillAmount(targetGaugeValue);
			isValueReached = false;
		}

		Debug.Log($"GetGaugeIntegerPercent : {comboGuageBarController.GetGaugeIntegerPercent()}");
		Debug.Log($"hpGuageBarController : {hpGuageBarController.GetGaugeIntegerPercent()}");
		Debug.Log($"bossHpGuageBarController : {bossHpGuageBarController.GetGaugeIntegerPercent()}");
		Debug.Log($"enemyGauageBarrController : {enemyGauageBarrController.GetGaugeIntegerPercent()}");
	}



	void NumberImageValueChanger()
	{
		int randValue = Random.Range(0, 999);

		numberImageLoader.SetNumber(randValue);
	}

	void TextValueChanger()
	{
		characterDialogController.SetCharactorText($"���ع��� ��λ��� ������ �⵵��\r\n�ϴ����� �����ϻ� �츮���� ����\r\n����ȭ ��õ�� ȭ������ ���ѻ�� �������� ����\r\n�����ϼ�. ���� ���� �� �ҳ��� ö���� �θ� ��\r\n�ٶ����� �Һ����� �츮��� �ϼ�\r\n����ȭ ��õ�� ȭ������ ���ѻ�� �������� ���̺����ϼ� \r\n");
	}

	void TextValueChangerReturn()
	{
		if (characterDialogController.GetTextEnd())
		{
			if (isAdWard)
			{
				characterDialogController.SetTypingDelay(0.001f);
				characterDialogController.SetCharactorText($"�������... ����¯......\n�������... ����¯......\n�������... ����¯......\n�������... ����¯......\n�������... ����¯......\n�������... ����¯......\n�������... ����¯......\n");
				isAdWard = false;
			} else
			{
				characterDialogController.SetTypingDelay(0.05f);
				characterDialogController.SetCharactorText($"���ع��� ��λ��� ������ �⵵��\r\n�ϴ����� �����ϻ� �츮���� ����\r\n����ȭ ��õ�� ȭ������ ���ѻ�� �������� ����\r\n�����ϼ�. ���� ���� �� �ҳ��� ö���� �θ� ��\r\n�ٶ����� �Һ����� �츮��� �ϼ�\r\n����ȭ ��õ�� ȭ������ ���ѻ�� �������� ���̺����ϼ� \r\n");
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
