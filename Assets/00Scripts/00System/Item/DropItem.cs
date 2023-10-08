using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
	[SerializeField, Header("Rotate 애니메이션")] 
	private float rotateSpeed = .0f;

	[SerializeField, Space(10), Header("Up Down 애니메이션")]
	private float upDownSpeed = .0f;
	
	[SerializeField]
	private float upDownScope = .0f;

	private int currentBoundCount = 0;
	private int maxBoundCount = 0;

	private float movementResult = .0f;

	private bool isPlayAnimation = false;
	private bool isUsedGetPlayer = false;

	private Vector3 animStartPos;

	private void Awake()
	{
		isPlayAnimation = false;
		isUsedGetPlayer = false;

	}

	private void Update()
	{
		if (isPlayAnimation)
		{
			PlayRotateAnimation();
			
			PlayUpDownAnimation();
		}
	}

	public void Play()
	{
		StartDropAnimation();
	}

	private void StartDropAnimation()
	{
		animStartPos = transform.position;
		
		isPlayAnimation = true;
	}

	private void PlayRotateAnimation()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
	}

	private void PlayUpDownAnimation()
	{
		var animPos = animStartPos;

		movementResult += Time.deltaTime * upDownSpeed;
		animPos.y += Mathf.Sin(movementResult + 1) * upDownScope;
		
		transform.position = animPos;
	}
}
