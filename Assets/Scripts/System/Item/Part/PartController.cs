using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartController : MonoBehaviour
{
	[Header("장착 중인 파츠")]
	public List<Part> equipPart;
	private const int MaxEquipCount = 4;

	private PlayerController ownerUnit;

	private StatusManager manager = new();
	private OriginStatus status;

	private float playerGauge = .0f;

	// Test용 코드 <- Epic Monster가 구현되지 않아서 부품 설정을 위함
	[Header("테스트용 파츠")]
	public List<Part> testPart;

	private void Awake()
	{
		status = ScriptableObject.CreateInstance<OriginStatus>();

		status.AutoGenerator();

		manager.SetStatus(status.GetStatus());

		TryGetComponent(out ownerUnit);
		
		ownerUnit.comboGaugeSystem.OnGaugeChanged.AddListener(OnGaugeChanged);
	}

	private void Start()
	{
		foreach (var VARIABLE in testPart)
		{
			EquipPart(VARIABLE);
		}
	}

	public void EquipPart(Part part)
	{
		if (equipPart.Count >= MaxEquipCount)
		{
			return;
		}

		if (part.PartItemData.PartActivation <= playerGauge)
		{
			part.SetActive(true);
			RunPassive(part);
		}

		equipPart.Add(part);
	}

	public void UnequipPart(Part part)
	{
		if(equipPart.Count <= 0)
		{
			return;
		}

		if (part.PartItemData.PartType == PartTriggerType.PASSIVE)
		{
			StopPassive(part);
		}
		else
		{
			StopActive(part);
		}

		equipPart.Remove(part);
	}

	private void OnGaugeChanged(float gauge)
	{
		playerGauge = gauge;

		if (equipPart.Count <= 0)
		{
			return;
		}
		
		foreach(var part in equipPart)
		{
			var partActivation = part.PartItemData.PartActivation;

			if(partActivation <= gauge && !part.GetActive())
			{
				part.SetActive(true);

				RunPassive(part);
			}
			else if (partActivation > gauge && part.GetActive())
			{
				part.SetActive(false);
				
				StopPassive(part);
			}
		}
	}

	// Passive

	private void RunPassive(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if (passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		var partData = passivePart.GetData();

		manager.AddStatus(partData.status);
		ownerUnit.playerData.status.AddStatus(partData.status);
	}

	private void StopPassive(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if(passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		var partData = passivePart.GetData();

		manager.SubStatus(partData.status);
		ownerUnit.playerData.status.SubStatus(partData.status);
	}

	// Active

	private void RunActive(Part part)
	{

	}

	private void StopActive(Part part)
	{

	}
}
