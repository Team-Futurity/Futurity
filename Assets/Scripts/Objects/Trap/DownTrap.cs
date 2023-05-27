using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrap : TrapBehaviour
{
	private DownTrapFall fall;

	public float fallObjYDistance = .0f;
	
	private void Awake()
	{
		TryGetComponent(out fall);
		
		if (fall is null)
		{
			FDebug.Log($"fallObj가 존재하지 않습니다.");
		}
		else
		{
			if (fall.gameObject.activeSelf)
			{
				fall.gameObject.SetActive(false);
			}
			
			var fallPos = transform.position;
			fallPos.y += fallObjYDistance;

			fall.SetPos(fallPos);
			fall.SetOwner(GetComponent<UnitBase>());
		}
	}
	
	public override void ActiveTrap(List<UnitBase> units)
	{
		trapStart?.Invoke();
		
		StartProcess();
	}

	private void StartProcess()
	{
		if (!fall.GetFalling())
		{
			fall.StartFall();
		}
	} 
}
