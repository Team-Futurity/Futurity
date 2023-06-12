using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적의 HP 바를 관리하고 제어하는 클래스입니다.
/// </summary>
public class EnemyHpBarController : MonoBehaviour
{
	// 만약 인스턴스화 되지 않았다면 enemyHpBar에 값을 넣어주세요.
	[SerializeField]
	[Header("만약 인스터스화 되지 않았다면 enemyHpBar에 값을 넣어주세요")]
	private GameObject enemyHpBar;

	// 만약 인게임에 HpBar가 존재한다면 currentHpBar에 값을 넣어주세요.
	[Space(10)]
	[SerializeField]
	[Header("만약 인게임에 HpBar가 존재한다면 currentHpBar에 값을 넣어주세요")]
	public GameObject currentHpBar;

	[SerializeField]
	private Transform canvasTrs;

	[Space(30)]
	[SerializeField]
	private Vector2 hpBarPosition;

	private EnemyController ec;



	/// <summary>
	/// 초기화 작업을 합니다. 만약 currentHpBar가 null이라면 새로운 창을 열어 대입합니다.
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
	/// 매 프레임 적의 월드 위치에 따라 HP 바의 위치를 갱신합니다.
	/// </summary>
	void Update()
	{
		Vector3 enemyWorldPosition = Camera.main.WorldToScreenPoint(transform.position) + (Vector3)hpBarPosition;
		if(ec.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() > 0 && currentHpBar != null)
			currentHpBar.transform.position = enemyWorldPosition;
	}

	/// <summary>
	/// 적의 HP 바의 게이지를 설정합니다.
	/// </summary>
	/// <param name="setValue">게이지를 설정할 값입니다.</param>
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
