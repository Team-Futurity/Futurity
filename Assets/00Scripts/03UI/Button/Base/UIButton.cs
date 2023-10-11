using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class UIButton : MonoBehaviour
{
	[field: SerializeField]
	public List<UISwap> swapList { get; private set; }

	public int layerOrder = 0;

	protected abstract void ActiveFunc();

	public void Active()
	{
		ActiveFunc();
	}

	public void Select(bool isOn)
	{
		SwapResources(isOn);
	}

	private void SwapResources(bool isOn)
	{
		for(int i= 0;i < swapList.Count; ++i)
		{
			swapList[i].Swap(isOn);
		}
	}
}
