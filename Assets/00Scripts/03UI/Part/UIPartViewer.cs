using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPartViewer : MonoBehaviour
{
	// 0 ~ 2 : Passive
	// 3	 : Active
	[SerializeField]
	private Image[] partImages;

	[SerializeField]
	private PartSystem partSystem;

	private const int Active_Icon_Index = 3;

	private void Awake()
	{
		if(partSystem == null)
		{
			FDebug.Log($"Part System�� �������� �ʽ��ϴ�.", GetType());

			partSystem = GameObject.Find("PlayerPerfab").GetComponent<PartSystem>();

			if(partSystem == null)
			{
				FDebug.Log($"�� ������ Part System�� ã�� �� �����ϴ�.", GetType());
			}
		}
	}

	public void SetPassiveIcon(int index, Sprite iconImage)
	{
		partImages[index].sprite = iconImage;
	}

	public void SetActiveIcon(Sprite iconImage)
	{
		partImages[Active_Icon_Index].sprite = iconImage;
	}

	public void ClearAll()
	{
		for(int i=0;i<partImages.Length; ++i)
		{
			if (partImages[i].sprite != null)
			{
				partImages[i].sprite = null;
			}
		}
	}

}
