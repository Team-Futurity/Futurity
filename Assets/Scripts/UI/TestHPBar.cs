using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHPBar : MonoBehaviour
{
	[SerializeField] private EnemyController enemy;
	[SerializeField] private Canvas canvas;
	[SerializeField] private Slider slider;
	[SerializeField] private Vector3 sliderPos;

	[HideInInspector] public Slider copySlider;
	private bool isSpawning;
	private float curTime;

	private void Start()
	{
		enemy = GetComponent<EnemyController>();
		curTime = 0f;
		if(slider != null)
		{
			copySlider = Instantiate(slider, enemy.transform.position + sliderPos, Quaternion.identity, canvas.transform);
			copySlider.value = 1;
			copySlider.gameObject.SetActive(false);
			isSpawning = true;
		}
	}

	void Update()
    {
		if(isSpawning)
		{
			curTime += Time.deltaTime;
			if(curTime > enemy.maxSpawningTime)
			{
				copySlider.gameObject.SetActive(true);
				isSpawning = false;
			}
		}
		if(slider != null)
		{
			copySlider.value = enemy.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() / enemy.enemyData.status.GetStatus(StatusType.MAX_HP).GetValue();
			copySlider.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + sliderPos);
			if (enemy.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
				copySlider.gameObject.SetActive(false);
		}
	}
}
