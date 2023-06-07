using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraditTextMover : MonoBehaviour
{
	[SerializeField]
	private List<string> craditTexts = new List<string>();

	[SerializeField]
	private Vector3 craditStartPosition = Vector3.zero;

	private float craditDelay;
	private WaitForSeconds craditDelayWait;

	private void Start()
	{
		craditDelayWait = new WaitForSeconds(craditDelay);
	}

	private void FixedUpdate()
	{
		
	}

	private void CraditPooling()
	{
		ObjectPoolManager<Transform> a;
	}

	IEnumerator craditWriter()
	{
		yield return craditDelayWait;
	}
}
