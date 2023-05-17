using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.UI;

public class NumberImageLoader : MonoBehaviour
{
	[SerializeField]
	List<Image> numberLoadField;
	[SerializeField]
	List<Sprite> numberSprite;


	public void SetNumber(int num)
	{
		int i1 = num % 10;
		int i10 = (num / 10) % 10;
		int i100 = num / 100;

		Debug.Log($"{gameObject.name}ÀÇ SetNumber : {i1}{i10}{i100}");

		numberLoadField[0].sprite = numberSprite[i1];
		numberLoadField[1].sprite = numberSprite[i10];
		numberLoadField[2].sprite = numberSprite[i100];
	}
}
