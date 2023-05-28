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
		while (true) // ���� �ݺ�
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
