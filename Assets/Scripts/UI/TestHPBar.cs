using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHPBar : MonoBehaviour
{
	public UnitBase unit;
	public Slider slider;

	private void Start()
	{
		
	}

	void Update()
    {
		slider.value = unit.status.GetStatus(StatusType.CURRENT_HP).GetValue() / unit.status.GetStatus(StatusType.MAX_HP).GetValue();
		slider.transform.rotation = Camera.main.transform.rotation;
	}
}
