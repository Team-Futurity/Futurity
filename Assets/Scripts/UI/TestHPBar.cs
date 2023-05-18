using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHPBar : MonoBehaviour
{
	public Enemy enemy;
	public Slider slider;

	private void Start()
	{
		
	}

	void Update()
    {
		slider.value = enemy.status.GetStatus(StatusType.CURRENT_HP).GetValue() / enemy.status.GetStatus(StatusType.MAX_HP).GetValue();
		slider.transform.rotation = Camera.main.transform.rotation;
	}
}
