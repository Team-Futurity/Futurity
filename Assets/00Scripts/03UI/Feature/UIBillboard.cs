using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
	Camera mainCam;

	private void Awake()
	{
		mainCam = Camera.main;
	}
	public void LateUpdate()
	{
		transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
	}
}
