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
	private SpriteRenderer prevSpriteRenderer;
	private Material mat;
	
	private AlphaChanger alphaChanger;
	private MaterialAlphaChanger matChanger;
	private ObjectDisable objectDisable;

	private void Start()
	{
		playerPos = GameObject.FindWithTag("Player").transform;
	}

	private void Update()
	{
		Vector3 dir = (playerPos.position - rayOffset.position).normalized;
		Ray ray = new Ray(rayOffset.position, dir);
		
		SpritePenetrate(ray);
		MaterialPenetrate(ray);
		ObjectDisable(ray);
	}

	private void SpritePenetrate(Ray ray)
	{
		if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
		{
			if (prevSpriteRenderer != null)
			{
				prevSpriteRenderer.color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
			}

			if (hit.collider.TryGetComponent(out SpriteRenderer spriteRenderer) == true)
			{
				prevSpriteRenderer = spriteRenderer;
				spriteRenderer.color = new Color(255f, 255f, 255f, newAlpha);
				
				ClearMaterialChanger();
			}
			else if (hit.collider.TryGetComponent(out alphaChanger))
			{
				alphaChanger.SetObjectAlpha(newAlpha);
				
				ClearMaterialChanger();
			}
		}
		else
		{
			if (prevSpriteRenderer != null)
			{
				Color color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
				prevSpriteRenderer.color = color;

				prevSpriteRenderer = null;
			}

			if (alphaChanger != null)
			{
				alphaChanger.SetObjectAlpha(ORIGIN_ALPHA);
				alphaChanger = null;
			}
		}
	}

	private void MaterialPenetrate(Ray ray)
	{
		if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
		{
			if (hit.collider.TryGetComponent(out matChanger) == true)
			{
				matChanger.ChangeAlpha(true);
				ClearSpriteRenderer();
			}
		}
		else
		{
			if (matChanger != null)
			{
				matChanger.ChangeAlpha(false);
				matChanger = null;
			}
		}
	}

	private void ObjectDisable(Ray ray)
	{
		if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
		{
			if (hit.collider.TryGetComponent(out objectDisable) == true)
			{
				objectDisable.SetActiveObject(false);
			}
		}
		else
		{
			if (objectDisable != null)
			{
				objectDisable.SetActiveObject(true);
				objectDisable = null;
			}
		}
	}

	private void ClearMaterialChanger()
	{
		if (matChanger == null)
		{
			return;
		}
		
		matChanger.ChangeAlpha(false);
		matChanger = null;
	}

	private void ClearSpriteRenderer()
	{
		if (alphaChanger != null)
		{
			alphaChanger.SetObjectAlpha(ORIGIN_ALPHA);
			alphaChanger = null;
		}

		if (prevSpriteRenderer != null)
		{
			prevSpriteRenderer.color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
			prevSpriteRenderer = null;
		}
	}
}
