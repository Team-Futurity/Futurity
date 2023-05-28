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
		curTime = 0f;
		copySlider = Instantiate(slider, enemy.transform.position + sliderPos, Quaternion.identity, canvas.transform);
		copySlider.value = 1;
		copySlider.gameObject.SetActive(false);
		isSpawning = true;
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
		copySlider.value = enemy.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() / enemy.enemyData.status.GetStatus(StatusType.MAX_HP).GetValue();
		copySlider.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + sliderPos);
	}
}
