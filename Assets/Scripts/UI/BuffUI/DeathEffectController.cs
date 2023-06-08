using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// �÷��̾��� ��� ��, ī�޶� �� �� �� ���ο� ��� ȿ���� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class DeathEffectController : MonoBehaviour
{
	public Transform playerTransform;
	public Transform cameraTransform;
	public float zoomInSpeed = 2f;
	public float slowMotionFactor = 0.5f;
	public float SizeClipTarget = 2; // ī�޶��� Size Ŭ���� �÷��� ��ǥ ��

	private Camera cameraComponent; // ī�޶� ������Ʈ
	private CameraController cameraController; // ī�޶� ������Ʈ
	private float initialSize; // �ʱ� Size ��

	[SerializeField]
	private WindowOpenController windowOpenController;

	public PostProcessVolume volume; // PostProcessVolume ������Ʈ�� ����
	private ColorGrading colorGrading; // ColorGrading ����Ʈ

	/// <summary>
	/// ������Ʈ�� �ʱ� Size ���� �����մϴ�.
	/// </summary>
	void Start()
	{
		cameraComponent = cameraTransform.GetComponent<Camera>();
		cameraController = cameraTransform.GetComponent<CameraController>();
		initialSize = cameraComponent.orthographicSize;

		windowOpenController = GetComponent<WindowOpenController>();

		volume.profile.TryGetSettings(out colorGrading);
	}

	/// <summary>
	/// �÷��̾ ������� �� ȣ��Ǵ� �޼����Դϴ�.
	/// ���ο� ��� �� ī�޶� �� �� ȿ���� �����ϰ�, ���� ���� â�� ���ϴ�.
	/// </summary>
	public void PlayerDeath()
	{
		Time.timeScale = slowMotionFactor;
		cameraController.enabled = false;

		StartCoroutine(ZoomInAndSlowMotion());

		StartCoroutine(OpenGameOverWindow());
	}

	/// <summary>
	/// �÷��̾� ��� ��, ī�޶� �� �� �� ���ο� ��� ȿ���� �����ϴ� �ڷ�ƾ�Դϴ�.
	/// </summary>
	IEnumerator ZoomInAndSlowMotion()
	{
		Debug.Log("PlayerDeath");

		colorGrading.saturation.value = 0f;
		Vector3 initialCameraPos = cameraTransform.position;
		Vector3 targetCameraPos = playerTransform.position;

		float journeyLength = Vector3.Distance(initialCameraPos, targetCameraPos);
		float startTime = Time.time;

		while (cameraTransform.position != targetCameraPos || cameraComponent.orthographicSize != SizeClipTarget)
		{
			float distCovered = (Time.time - startTime) * zoomInSpeed;
			float fractionOfJourney = distCovered / journeyLength;

			if (cameraTransform.position != targetCameraPos)
			{
				cameraTransform.position = Vector3.Lerp(initialCameraPos, targetCameraPos, fractionOfJourney);
			}

			if (cameraComponent.orthographicSize != SizeClipTarget)
			{
				cameraComponent.orthographicSize = Mathf.Lerp(initialSize, SizeClipTarget, fractionOfJourney);
			}

			yield return null;
		}
	}

	/// <summary>
	/// ���� ���� â�� ���� ȭ���� ���̵� ��/�ƿ��ϴ� �ڷ�ƾ�Դϴ�.
	/// </summary>
	IEnumerator OpenGameOverWindow()
	{
		yield return new WaitForSeconds(2f);
		yield return FadeManager.Instance.FadeCoroutineStart(false, 1, Color.black);
		windowOpenController.WindowDeactiveOpen();
		yield return FadeManager.Instance.FadeCoroutineStart(true, 1, Color.black);
	}

	/// <summary>
	/// ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ �����·� �����ϴ�.
	/// </summary>
	void OnDisable()
	{
		colorGrading.saturation.value = 100f;
	}
}
