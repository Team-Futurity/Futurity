using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPenetrate : MonoBehaviour
{
	// [Header("Component")] 
	// public Transform playerTf;
	//
	// [Header("Penetrate")]
	// [SerializeField] private LayerMask visibleLayer;
	// [SerializeField] private string colorFieldName;
	// [SerializeField, Range(0, 1)] private float opacity;
	// [SerializeField] private float calcThreshold;
	// private RaycastHit[] penetrateRaycastHit;
	// private Material[] penetratedMaterial;
	// private Color[] penetratedColor;
	// private Vector3 prevPosition;
	//
	// [Header("Correction")]
	// [SerializeField] private int decimalCount;
	// private Camera mainCam;
	//
	// private void Awake()
	// {
	// 	mainCam = Camera.main;
	// }
	//
	// private void FixedUpdate()
	// {
	// 	SetPenetrate();
	// }
	//
	// #region Perspective Effect
	// private void SetPenetrate()
	// {
	// 	if ((prevPosition - transform.position).magnitude <= calcThreshold)
	// 	{
	// 		return;
	// 	}
	// 	
	// 	if (penetratedMaterial != null)
	// 	{
	// 		for (int length = 0; length < penetratedMaterial.Length; length++)
	// 		{
	// 			if (penetratedMaterial[length] == null)
	// 			{
	// 				continue;
	// 			}
	//
	// 			penetratedColor[length].a = 1f;
	// 			penetratedMaterial[length].SetColor(colorFieldName, penetratedColor[length]);
	// 		}
	// 	}
	// 	
	// 	Vector3 targetViewportPoint = mainCam.WorldToViewportPoint(playerTf.transform.position);
	// 	Ray ray = mainCam.ViewportPointToRay(targetViewportPoint);
	// 	Vector3 targetVec = playerTf.position - ray.origin;
	// 	
	// 	penetrateRaycastHit = Physics.RaycastAll(ray, targetVec.magnitude, visibleLayer);
	// 	if (penetrateRaycastHit.Length > 0)
	// 	{
	// 		penetratedMaterial = new Material[penetrateRaycastHit.Length];
	// 		penetratedColor = new Color[penetrateRaycastHit.Length];
	// 		for (int length = 0; length < penetrateRaycastHit.Length; length++)
	// 		{
	// 			if (penetrateRaycastHit[length].transform.gameObject == playerTf.gameObject)
	// 			{
	// 				continue;
	// 			}
	//
	// 			var wallRenderer = penetrateRaycastHit[length].transform.GetComponent<Renderer>();
	// 			if (wallRenderer == null)
	// 			{
	// 				continue;
	// 			}
	// 			penetratedMaterial[length] = wallRenderer.material;
	//
	// 			if (!penetratedMaterial[length].HasColor(colorFieldName))
	// 			{
	// 				continue;
	// 			}
	// 			penetratedColor[length] = penetratedMaterial[length].GetColor(colorFieldName);
	// 			penetratedColor[length].a = opacity;
	// 			penetratedMaterial[length].SetColor(colorFieldName, penetratedColor[length]);
	// 		}
	// 	}
	//
	// 	prevPosition = GetTruncatedVector(transform.position);
	// }
	//
	// private Vector3 GetTruncatedVector(Vector3 originVector)
	// {
	// 	Vector3 truncatingVector = Vector3.zero;
	// 	float truncatingValue = Mathf.Pow(10, decimalCount);
	//
	// 	truncatingVector.x = Mathf.Floor(originVector.x * truncatingValue) / truncatingValue;
	// 	truncatingVector.y = Mathf.Floor(originVector.y * truncatingValue) / truncatingValue;
	// 	truncatingVector.z = Mathf.Floor(originVector.z * truncatingValue) / truncatingValue;
	//
	// 	return truncatingVector;
	// }
	// #endregion

	[Header("활성화 여부")] 
	[SerializeField] private bool isEnable = false;

	[Header("알파값 조정")] 
	[SerializeField] private float newAlpha = 0.2f;

	[Header("검출 레이어")] 
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private float rayLength = 30.0f;

	private const float ORIGIN_ALPHA = 1.0f;
	private Camera mainCamera;
	private Vector3 screenCenter = Vector3.zero;
	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		mainCamera = Camera.main;

		screenCenter = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
	}

	private void Update()
	{
		if (isEnable == false)
		{
			return;
		}

		Ray ray = mainCamera.ScreenPointToRay(screenCenter);

		if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
		{
			spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

			if (spriteRenderer != null)
			{
				Color color = new Color(255f, 255f, 255f, newAlpha);
				spriteRenderer.color = color;
			}
		}
		else
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.color = new Color(255f, 255f, 255f, ORIGIN_ALPHA);
				spriteRenderer = null;
			}
		}
	}
}
