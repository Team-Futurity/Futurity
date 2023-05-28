using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumberImageLoader : MonoBehaviour
{
	[SerializeField]
	List<Material> numberComboMaterial;
	[SerializeField]
	List<ParticleSystem> numberComboParticle;

	int[] currntCombos = new int[] {0,0,0 };

	[SerializeField]
	List<Texture> numberSprite;

	public void SetNumber(int num)
	{
		currntCombos[0] = num / 100;
		currntCombos[1] = (num / 10) % 10;
		currntCombos[2] = num % 10;

		FDebug.Log($"{gameObject.name}ÀÇ SetNumber : {currntCombos[0]}{currntCombos[1]}{currntCombos[2]}");

		for (int i = 0; i < 3; i++)
		{
			numberComboMaterial[i].SetTexture("_maintex", numberSprite[currntCombos[i]]);
			numberComboParticle[i].Stop();
			numberComboParticle[i].Play();

			FDebug.Log($"{numberComboMaterial[i].name}ÀÇ mainTexture : {numberComboMaterial[i].GetTexture("_maintex")}");
		}
	}
}
