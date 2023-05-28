using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarController : MonoBehaviour
{
	[SerializeField]
	[Header("���� �ν��ͽ�ȭ ���� �ʾҴٸ� enemyHpBar�� ���� �־��ּ���")]
	private GameObject enemyHpBar;
	private GameObject currentHpBar;
	private int poolingHpBarNum;

	[Space(30)]
	[SerializeField]
	private Vector2 hpBarPosition;



	void Start()
    {
		poolingHpBarNum = WindowManager.Instance.WindowPooling(enemyHpBar);
		currentHpBar = WindowManager.Instance.WindowOpen(poolingHpBarNum, hpBarPosition, Vector3.zero);
    }

    void Update()
    {
		Vector3 enemyWorldPosition = Camera.main.WorldToScreenPoint(transform.position) + (Vector3)hpBarPosition;

		currentHpBar.transform.position = enemyWorldPosition;
	}

	public void SetHpBarFill(float setValue)
	{
		currentHpBar.GetComponent<GaugeBarController>().SetGaugeFillAmount(setValue);
	}
}
