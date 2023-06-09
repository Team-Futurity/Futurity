using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePart : Part, IPassive
{
	private BuffProvider buffProvider;
	private BuffSystem buffSystem;

	[field: Space(10)]
	[field: Header("파츠 스테이터스")]
	[field: SerializeField] public OriginStatus PartStatus { get; private set; }

	[field: Space(10)]
	[field: Header("파츠 효과 적용 타입")]
	[field: SerializeField] public PassiveApplyType PartApplyType { get; private set; }

	private void Awake()
	{
		TryGetComponent(out buffSystem);

		if (PartItemData is null)
		{
			FDebug.Log($"{PartItemData.GetType()}이(가) 존재하지 않습니다.");
			Debug.Break();
		}

		if (PartItemData.PartType is not PartTriggerType.PASSIVE)
		{
			FDebug.Log($"{PartItemData.PartType}는 현재 Script와 데이터가 맞지 않습니다.");
			Debug.Break();
		}

		if (PartUIData is null)
		{
			FDebug.Log($"{PartUIData.GetType()}이(가) 존재하지 않습니다.");
			Debug.Break();
		}
	}

	public PassiveData GetData()
	{
		var data = new PassiveData();

		data.status = PartStatus.GetStatus();
		data.buffName = BuffName.NONE;

		return data;
	}
}