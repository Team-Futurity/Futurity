using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarController : MonoBehaviour
{
	[SerializeField]
	[Header("만약 인스터스화 되지 않았다면 enemyHpBar에 값을 넣어주세요")]
	private GameObject enemyHpBar;
	[Space(10)]
	[SerializeField]
	[Header("만약 인게임에 HpBar가 존재한다면 currentHpBar에 값을 넣어주세요")]
	private GameObject currentHpBar;

	[Space(30)]
	[SerializeField]
	private Vector2 hpBarPosition;



	void Start()
    {
		if(currentHpBar == null)
		currentHpBar = WindowManager.Instance.WindowTopOpen(enemyHpBar, hpBarPosition, Vector3.zero);
    }

    void Update()
    {
		Vector3 enemyWorldPosition = Camera.main.WorldToScreenPoint(transform.position);
		enemyWorldPosition.x -= Screen.width / 2;
		enemyWorldPosition.y -= Screen.height / 2;

		currentHpBar.transform.position = enemyWorldPosition;
	}

	public void SetHpBarFill(float setValue)
	{
		currentHpBar.GetComponent<GaugeBarController>().SetGaugeFillAmount(setValue);
	}
}
