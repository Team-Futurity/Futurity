using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassivePart : Part, IPassive
{
	private BuffSystem buffSystem;

	private OriginStatus originStatus;

	private UnityEvent<OriginStatus> OnSet;
	private UnityEvent OnRemove;

	[field: Space(10)]
	[field: Header("���� �������ͽ�")]
	[field: SerializeField] public OriginStatus PartStatus { get; private set; }

	[field: Space(10)]
	[field: Header("���� ȿ�� ���� Ÿ��")]
	[field: SerializeField] public PassiveApplyType PartApplyType { get; private set; }

	private void Awake()
	{
		TryGetComponent(out buffSystem);

		if (PartItemData is null)
		{
			FDebug.Log($"{PartItemData.GetType()}��(��) �������� �ʽ��ϴ�.");
			Debug.Break();
		}

		if (PartItemData.PartType is not PartTriggerType.PASSIVE)
		{
			FDebug.Log($"{PartItemData.PartType}�� ���� Script�� �����Ͱ� ���� �ʽ��ϴ�.");
			Debug.Break();
		}

		if(PartUIData is null)
		{
			FDebug.Log($"{PartUIData.GetType()}��(��) �������� �ʽ��ϴ�.");
			Debug.Break();
		}
	}

	public PassiveData GetData()
	{
		var data = new PassiveData();

		data.status = PartStatus.GetStatus();
		data.buffName = BuffNameList.NONE;

		return data;
	}
}