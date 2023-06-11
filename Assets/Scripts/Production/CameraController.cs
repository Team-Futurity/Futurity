using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("카메라가 추적할 대상입니다.")]
    public Transform target;
	
    [Tooltip("추적 대상에게서 얼마나 떨어진 위치에 카메라가 있는지를 나타냅니다.")]
    public Vector3 offset;
	[Tooltip("카메라가 이동하는 동안 보정해줄 값입니다.")]
	public float moveOffset;

	private Vector3 prevTargetVector;
	private PlayerController playerController;
	private Vector3 moveDir;

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
	[SerializeField] private float calcThreshold; // 연산할 문턱값 
	private RaycastHit[] penetrateRaycastHit;
	private Material[] penetratedMaterial;
	private Color[] penetratedColor;
	private Vector3 prevPosition;

	[Header("Correction")]
	[SerializeField] private int decimalCount;


	[SerializeField] private float camSpeed;

	private Vector3 cameraVelocity;


	private void Start()
	{
		initialPos = target.position + offset;
		isVibrate = false;
		prevTargetVector = GetTruncatedVector(target.transform.position);
		transform.position = GetTruncatedVector(target.position) + offset;
		playerController = target.GetComponent<PlayerController>();
	}

	private Vector3 GetTruncatedVector(Vector3 originVector)
	{
		float x, y, z;
		float trucncatingValue = Mathf.Pow(10, decimalCount);

		x = Mathf.Floor(originVector.x * trucncatingValue) / trucncatingValue;
		y = Mathf.Floor(originVector.y * trucncatingValue) / trucncatingValue;
		z = Mathf.Floor(originVector.z * trucncatingValue) / trucncatingValue;

		return new Vector3(x, y, z);
	}

	private void FixedUpdate()
    {
		// 카메라 위치 조정
		SetCameraPosition();

		// 투시
		SetPenetrate();
	}

	private void Update()
	{
		if(playerController != null) { moveDir = playerController.moveDir; }

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

	private void LateUpdate()
	{
		// 카메라 위치 조정
		/*SetCameraPosition();*/

		
		prevTargetVector = GetTruncatedVector(target.position);
	}

	public void SetVibration(float time, float curvePower = 0.1f, float randomPower = 0.1f)
	{
		vibrationPower = randomPower;
		this.curvePower = curvePower;
		initialPos = transform.position;
		targetTime = time;
		isVibrate = true;
	}

	public void SetCameraPosition()
	{
		var targetVector = GetTruncatedVector(target.position);
		var currentVector = targetVector + offset;

		//if (prevTargetVector == targetVector) {  }
		//if (!playerController){ transform.position = currentVector; return; }
			
		//var targetPosition = moveDir == Vector3.zero ? currentVector : (currentVector + moveOffset * moveDir);
		//var targetPosition = moveDir == Vector3.zero ? currentVector : Vector3.zero;
		
		/*var alterX = Mathf.Abs(targetPosition.x) * moveDir.x;
		var alterY = Mathf.Abs(targetPosition.y) * moveDir.y;
		var alterZ = Mathf.Abs(targetPosition.z) * moveDir.z;

		//.position += new Vector3(alterX, alterY, alterZ);
		//transform.position = currentVector; // 코드 오동작으로 임시로 덮어씌움
		targetPosition = moveDir == Vector3.zero ? targetPosition : new Vector3(alterX, alterY, alterZ);*/

		transform.position = Vector3.SmoothDamp(transform.position, currentVector, ref cameraVelocity, camSpeed * Time.deltaTime);
	}

	public void SetPenetrate()
	{
		// 투시 스크립트
		if ((prevPosition - transform.position).magnitude <= calcThreshold) { return; }

		// 초기화
		if (penetratedMaterial != null)
		{
			for (int length = 0; length < penetratedMaterial.Length; length++)
			{
				if (penetratedMaterial[length] == null) { continue; }

				penetratedColor[length].a = 1f;
				penetratedMaterial[length].SetColor(colorFieldName, penetratedColor[length]);
			}
		}

		// 계산
		Vector3 targetViewportPoint = Camera.main.WorldToViewportPoint(target.transform.position);
		Ray ray = Camera.main.ViewportPointToRay(targetViewportPoint);
		Vector3 targetVec = target.position - ray.origin;

		// 변경
		penetrateRaycastHit = Physics.RaycastAll(ray, targetVec.magnitude, visibleLayer);
		if (penetrateRaycastHit.Length > 0)
		{

			penetratedMaterial = new Material[penetrateRaycastHit.Length];
			penetratedColor = new Color[penetrateRaycastHit.Length];
			for (int length = 0; length < penetrateRaycastHit.Length; length++)
			{
				if (penetrateRaycastHit[length].transform.gameObject == target) { continue; }

				var renderer = penetrateRaycastHit[length].transform.GetComponent<Renderer>();
				if (renderer == null) { continue; }
				penetratedMaterial[length] = renderer.material;

				if (!penetratedMaterial[length].HasColor(colorFieldName)) { continue; }
				penetratedColor[length] = penetratedMaterial[length].GetColor(colorFieldName);
				penetratedColor[length].a = opacity;
				penetratedMaterial[length].SetColor(colorFieldName, penetratedColor[length]);
			}
		}

		prevPosition = GetTruncatedVector(transform.position);
	}
}
