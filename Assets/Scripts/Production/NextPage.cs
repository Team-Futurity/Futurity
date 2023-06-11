using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPage : MonoBehaviour
{
	public Animator anim;
	public GameObject page1;
	public GameObject page2;
	public bool isNext;

	private void Start()
	{
		isNext = false;
	}

	public void Update()
	{
		if(isNext){ return; }

		if(Input.anyKeyDown)
		{
			page1.SetActive(false);
			page2.SetActive(true);

			anim.SetTrigger("PressAnyKey");
		}
	}
}
