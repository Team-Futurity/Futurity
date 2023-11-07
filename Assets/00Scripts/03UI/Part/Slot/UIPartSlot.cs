using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIPartSlot : MonoBehaviour
{
	[SerializeField]
	private PartSystem partSystem;

	[Space(10), Header("Part ����")]
	public UIPassiveSlot[] passiveSlots;
	public UIActiveSlot activeSlot;

	private void Awake()
	{
		partSystem.onPartActive?.AddListener((index) =>
	   {
		   if(index == 3)
		   {
			  // activeSlot.SetSlot();
			   return;
		   }

		   passiveSlots[index].SetActivateImage(true);
	   });

		partSystem.onPartDeactive?.AddListener((index) =>
		{
			if(index == 3)
			{
				activeSlot.ClearSlot();
				return;
			}

			passiveSlots[index].SetActivateImage(false);
		});

		partSystem.onPartEquip?.AddListener((index, code) =>
		{
			if (index == 999) return;
			AddPartIcon(index, code);
		});
	}

	private void Start()
	{
		SyncToPartSystem();
	}

	public void AddPartIcon(int index, int partCode)
	{
		passiveSlots[index].SetSlot(LoadPassivePartIconImage(partCode));
	}

	public void SyncToPartSystem()
	{
		for(int i = 0; i <= 3; ++i)
		{
			if (!partSystem.IsIndexPartEmpty(i)) continue;

			if(i == 3)
			{
				activeSlot.ClearSlot();

				return;
			}

			passiveSlots[i].ClearSlot();
		}
	}


	#region Data Load

	private Sprite LoadPassivePartIconImage(int code)
	{
		var passiveData = Addressables.LoadAssetAsync<UIPassiveSelectData>(code.ToString()).WaitForCompletion();
		return passiveData.partIconSpr;
	}

	private Sprite LoadActivePartIconImage(int code)
	{
		return null;
	}

	#endregion

}
