using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIDialogController : MonoBehaviour, IControllerMethod
{
	[field: SerializeField] public UIDialogType DialogType { get; private set; }
	
	// Dialog Data
	// Dialog Text
	[field: SerializeField] public UIDialogText DialogText { get; private set; }
	// Dialog 

	private bool isActive;

	public UnityEvent onStart;
	public UnityEvent onEnd;

	private void Awake()
	{
		isActive = false;
	}

	// �׽�Ʈ�� �ڵ�
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Show();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Pass();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			DialogText.Stop();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			DialogText.Restart();
		}
	}

	#region IControllerMethod
	
	void IControllerMethod.Active()
	{
		
	}

	void IControllerMethod.Run()
	{
		
	}

	void IControllerMethod.Stop()
	{
		
	}
	
	#endregion

	public void Show()
	{
		DialogText.Show("�ӽ÷� �ؽ�Ʈ�� �ۼ��غ��ϴ�.");
	}
	
	public void Skip()
	{
		
	}

	public void Pass()
	{
		DialogText.Pass();
	}
}
