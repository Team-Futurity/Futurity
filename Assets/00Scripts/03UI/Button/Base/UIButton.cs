using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class UIButton : MonoBehaviour
{
	[field: SerializeField]
	public List<UISwap> swapList { get; private set; }


	public bool usedLeftRight = false;

	protected abstract void ActiveFunc();
	protected virtual void OnLeftActive() {}
	protected virtual void OnRightActive() {}

	private bool isSelected = false;

	public void Active()
	{
		ActiveFunc();
	}

	public void OnLeft()
	{
		OnLeftActive();
	}

	public void OnRight()
	{
		OnRightActive();
	}

	public void Select(bool isOn)
	{
		SwapResources(isOn);

		SelectActive(isOn);
	}

	public virtual void SelectActive(bool isOn)
	{

	}


	public void SetDefault()
	{
		for (int i = 0; i < swapList.Count; ++i)
		{
			swapList[i].SetDefaultImage();
		}
	}

	private void SwapResources(bool isOn)
	{
		for(int i= 0;i < swapList.Count; ++i)
		{
			swapList[i].Swap(isOn);
		}
	}
	
}
