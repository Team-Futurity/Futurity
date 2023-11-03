using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPenetrate : MonoBehaviour
{
	[Header("알파값 조정")] 
	[SerializeField] private float newAlpha = 0.2f;

	[Header("검출 레이어")] 
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private float rayLength = 30.0f;

	[Header("포지션 값")] 
	[SerializeField] private Transform rayOffset;
	private Transform playerPos;
	
	private const float ORIGIN_ALPHA = 1.0f;
	private SpriteRenderer spriteRenderer;
	private AlphaChanger alphaChanger;

	private void Start()
	{
		playerPos = GameObject.FindWithTag("Player").transform;
	}

	private void Update()
	{
		Vector3 dir = (playerPos.position - rayOffset.position).normalized;
		Ray ray = new Ray(rayOffset.position, dir);

		if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
		{
			if (hit.collider.TryGetComponent(out spriteRenderer) == true)
			{
				Color color = new Color(255f, 255f, 255f, newAlpha);
				spriteRenderer.color = color;
			}
			else if (hit.collider.TryGetComponent(out alphaChanger))
			{
				alphaChanger.SetObjectAlpha(newAlpha);
			}
		}
		else
		{
			if (spriteRenderer != null)
			{
				Color color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
				spriteRenderer.color = color;

				spriteRenderer = null;
			}

			if (alphaChanger != null)
			{
				alphaChanger.SetObjectAlpha(ORIGIN_ALPHA);
				alphaChanger = null;
			}
		}
	}
	
}
