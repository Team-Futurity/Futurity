using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarController : MonoBehaviour
{
	[SerializeField]
	[Header("���� �ν��ͽ�ȭ ���� �ʾҴٸ� enemyHpBar�� ���� �־��ּ���")]
	private GameObject enemyHpBar;
	[Space(10)]
	[SerializeField]
	[Header("���� �ΰ��ӿ� HpBar�� �����Ѵٸ� currentHpBar�� ���� �־��ּ���")]
	private GameObject currentHpBar;

	[Space(30)]
	[SerializeField]
	private Vector2 hpBarPosition;



	void Start()
    {
		if(currentHpBar == null)
		currentHpBar = WindowManager.Instance.WindowTopOpen(enemyHpBar, true, hpBarPosition, Vector3.zero);
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
