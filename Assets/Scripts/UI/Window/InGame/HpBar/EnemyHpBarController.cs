using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� HP �ٸ� �����ϰ� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class EnemyHpBarController : MonoBehaviour
{
	// ���� �ν��Ͻ�ȭ ���� �ʾҴٸ� enemyHpBar�� ���� �־��ּ���.
	[SerializeField]
	[Header("���� �ν��ͽ�ȭ ���� �ʾҴٸ� enemyHpBar�� ���� �־��ּ���")]
	private GameObject enemyHpBar;

	// ���� �ΰ��ӿ� HpBar�� �����Ѵٸ� currentHpBar�� ���� �־��ּ���.
	[Space(10)]
	[SerializeField]
	[Header("���� �ΰ��ӿ� HpBar�� �����Ѵٸ� currentHpBar�� ���� �־��ּ���")]
	public GameObject currentHpBar;

	[SerializeField]
	private Transform canvasTrs;

	[Space(30)]
	[SerializeField]
	private Vector2 hpBarPosition;

	private EnemyController ec;



	/// <summary>
	/// �ʱ�ȭ �۾��� �մϴ�. ���� currentHpBar�� null�̶�� ���ο� â�� ���� �����մϴ�.
	/// </summary>
	void Start()
	{
		if (currentHpBar == null)
			currentHpBar = WindowManager.Instance.DontUsedWindowOpen(enemyHpBar);

		ec = GetComponent<EnemyController>();

		currentHpBar.transform.parent = canvasTrs;

		EnemyManager.Instance.ActiveHpBar(this);
	}

	/// <summary>
	/// �� ������ ���� ���� ��ġ�� ���� HP ���� ��ġ�� �����մϴ�.
	/// </summary>
	void Update()
	{
		Vector3 enemyWorldPosition = Camera.main.WorldToScreenPoint(transform.position) + (Vector3)hpBarPosition;
		if(ec.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() > 0 && currentHpBar != null)
			currentHpBar.transform.position = enemyWorldPosition;
	}

	/// <summary>
	/// ���� HP ���� �������� �����մϴ�.
	/// </summary>
	/// <param name="setValue">�������� ������ ���Դϴ�.</param>
	public void SetHpBarFill(float setValue)
	{
		currentHpBar.GetComponent<GaugeBarController>().SetGaugeFillAmount(setValue);
	}

	public void DestroyHpBar()
	{
		Destroy(currentHpBar);
	}

	public void OnDestroy()
	{
		EnemyManager.Instance.DeactiveHpBar(this);
	}
}
