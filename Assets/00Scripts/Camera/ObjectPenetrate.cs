using System;
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
			spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

			if (spriteRenderer == null)
			{
				return;
			}
			
			Color color = new Color(255f, 255f, 255f, newAlpha);
			spriteRenderer.color = color;
		}
		else
		{
			if (spriteRenderer == null)
			{
				return;
			}
			
			spriteRenderer.color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
			spriteRenderer = null;
		}
	}

	private void OnDisable()
	{
		if (spriteRenderer == null)
		{
			return;
		}

		spriteRenderer.color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
	}
}
