using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberImageLoader : MonoBehaviour
{
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

	[SerializeField]
	private UnityEvent startSetNumberEvent;



	private void Awake()
	{
		for (int i = 0; i < ComboObjects.Count; i++)
		{
			ComboObjects[i].SetActive(false);
		}
	}

	public void SetNumber(int num)
	{
		return;

		startSetNumberEvent?.Invoke();

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

		FDebug.Log($"{gameObject.name}ÀÇ SetNumber : {currntCombos[0]}{currntCombos[1]}{currntCombos[2]}");


		for (int i = 0; i < numberComboMaterial.Count; i++)
		{
			numberComboMaterial[i].SetTexture("_maintex", numberSprite[currntCombos[i]]);

			FDebug.Log($"{numberComboMaterial[i].name}ÀÇ mainTexture : {numberComboMaterial[i].GetTexture("_maintex")}");
		}

		for (int i = 0; i < numberComboParticle.Count; i++)
		{
			numberComboParticle[i].Stop();
			numberComboParticle[i].Play();
		}

		comboDeactiveDelayCorutine = StartCoroutine(ComboDeactiveDelay());
	}

	IEnumerator ComboDeactiveDelay()
	{
		yield return new WaitForSeconds(10);

		for(int i = 0; i < ComboObjects.Count; i++)
		{
			ComboObjects[i].SetActive(false);
		}
	}
}
