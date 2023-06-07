using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Tooltip("ī�޶� ������ ����Դϴ�.")]
    public Transform target;
    [Tooltip("���� ��󿡰Լ� �󸶳� ������ ��ġ�� ī�޶� �ִ����� ��Ÿ���ϴ�.")]
    public Vector3 offset;

	[Header("Caemra Shake")]
    [SerializeField] private bool isVibrate;
    [SerializeField] private float vibrationPower;
    [SerializeField] private float targetTime;
	[SerializeField] private float curvePower;
	[SerializeField] private AnimationCurve curve;
	private Vector3 initialPos;
    private float curTime;

	[Header("Penetrate")]
	[SerializeField] private LayerMask visibleLayer;
	[SerializeField] private string colorFieldName;
	[SerializeField, Range(0, 1)] private float opacity;
	[SerializeField] private float calcThreshold; // ������ ���ΰ� 
	private RaycastHit[] penetrateRaycastHit;
	private Material[] penetratedMaterial;
	private Color[] penetratedColor;
	private Vector3 prevPosition;


	private void Start()
	{
		initialPos = target.position + offset;
		isVibrate = false;
	}

	private void FixedUpdate()
    {
		// ī�޶� ��ġ ����
        transform.position = target.position + offset;

		// ���� ��ũ��Ʈ

		if((prevPosition - transform.position).magnitude <= calcThreshold) { return; }

		// �ʱ�ȭ
		if (penetratedMaterial != null)
		{
			for (int length = 0; length < penetratedMaterial.Length; length++)
			{
				penetratedColor[length].a = 1f;
				penetratedMaterial[length].SetColor(colorFieldName, penetratedColor[length]);
			}
		}

		// ���
		Vector3 targetViewportPoint = Camera.main.WorldToViewportPoint(target.transform.position);
		Ray ray = Camera.main.ViewportPointToRay(targetViewportPoint);
		Vector3 targetVec = target.position - ray.origin;

		// ����
		penetrateRaycastHit = Physics.RaycastAll(ray, targetVec.magnitude, visibleLayer);
		if (penetrateRaycastHit.Length > 0)
		{
			penetratedMaterial = new Material[penetrateRaycastHit.Length];
			penetratedColor = new Color[penetrateRaycastHit.Length];
			for (int length = 0; length < penetrateRaycastHit.Length; length++)
			{
				penetratedMaterial[length] = penetrateRaycastHit[length].transform.GetComponent<Renderer>().material;

				if (!penetratedMaterial[length].HasColor(colorFieldName)) { continue; }
				penetratedColor[length] = penetratedMaterial[length].GetColor(colorFieldName);
				penetratedColor[length].a = opacity;
				penetratedMaterial[length].SetColor(colorFieldName, penetratedColor[length]);
			}
		}

		prevPosition = transform.position;
	}

	private void Update()
	{
		if (isVibrate)
		{
			if (targetTime > curTime)
			{
				float curGraph = curve.Evaluate(curTime / targetTime);
				curTime += Time.deltaTime;
				transform.position = initialPos + Vector3.right * (curGraph - 0.5f) * curvePower + Random.insideUnitSphere * vibrationPower;
			}
			else
			{
				curTime = 0;
				transform.position = initialPos;
				isVibrate = false;
			}
		}
	}

	public void SetVibration(float time, float curvePower = 0.1f, float randomPower = 0.1f)
	{
		vibrationPower = randomPower;
		this.curvePower = curvePower;
		initialPos = transform.position;
		targetTime = time;
		isVibrate = true;
	}
}
