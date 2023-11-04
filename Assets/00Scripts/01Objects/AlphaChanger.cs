using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChanger : MonoBehaviour
{
	[SerializeField] private List<SpriteRenderer> spriteRenderer;

	private void Start()
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			if (transform.GetChild(i).TryGetComponent(out SpriteRenderer wallRenderer))
			{
				spriteRenderer.Add(wallRenderer);
			}
		}
	}

	public void SetObjectAlpha(float alpha)
	{
		foreach (SpriteRenderer sprite in spriteRenderer)
		{
			Color color = new Color(255f, 255f, 255f, alpha);
			sprite.color = color;
		}
	}
}
