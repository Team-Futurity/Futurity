using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumberImageLoader : MonoBehaviour
{
	[Header("숫자 이미지를 로드하여 특정 시간 동안 보여주는 클래스")]
	[Space(20)]

	[SerializeField]
	private List<GameObject> ComboObjects;

	[SerializeField]
	private List<Material> numberComboMaterial;

	[SerializeField]
	private List<ParticleSystem> numberComboParticle;

	private int[] currntCombos = new int[] {0,0,0 };

	[SerializeField]
	private List<Texture> numberSprite;

	private Coroutine comboDeactiveDelayCorutine;

	private void Awake()
	{
		for (int i = 0; i < ComboObjects.Count; i++)
		{
			ComboObjects[i].SetActive(false);
		}
	}

	/// <summary>
	/// 콤보 숫자를 설정하고 활성화하는 메서드입니다. 설정된 숫자에 따라 콤보 숫자의 Material을 변경하며, 숫자가 활성화되면 ParticleSystem을 플레이합니다.
	/// </summary>
	/// <param name="num"></param>
	public void SetNumber(int num)
	{
		if (comboDeactiveDelayCorutine is not null)
		{
			StopCoroutine(comboDeactiveDelayCorutine);
		}

		for (int i = 0; i < ComboObjects.Count; i++)
		{
			ComboObjects[i].SetActive(true);
		}

		currntCombos[0] = num / 100;
		currntCombos[1] = (num / 10) % 10;
		currntCombos[2] = num % 10;

		FDebug.Log($"{gameObject.name}의 SetNumber : {currntCombos[0]}{currntCombos[1]}{currntCombos[2]}");


		for (int i = 0; i < numberComboMaterial.Count; i++)
		{
			numberComboMaterial[i].SetTexture("_maintex", numberSprite[currntCombos[i]]);

			FDebug.Log($"{numberComboMaterial[i].name}의 mainTexture : {numberComboMaterial[i].GetTexture("_maintex")}");
		}

		for (int i = 0; i < numberComboParticle.Count; i++)
		{
			numberComboParticle[i].Stop();
			numberComboParticle[i].Play();
		}

		if (gameObject.activeInHierarchy)
		{
			comboDeactiveDelayCorutine = StartCoroutine(ComboDeactiveDelay());
		}
	}

	/// <summary>
	/// 일정 시간 후에 콤보 숫자를 비활성화합니다.
	/// </summary>
	IEnumerator ComboDeactiveDelay()
	{
		yield return new WaitForSeconds(10);

		for(int i = 0; i < ComboObjects.Count; i++)
		{
			ComboObjects[i].SetActive(false);
		}
	}
}
