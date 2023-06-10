using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CraditGlitchEndTextController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textMeshProUGUI;

	[SerializeField]
	private UnityEvent EndTextEvent = null;

	[SerializeField]
	private float destroyTime;

	[SerializeField]
	Color textColor = Color.white;

	void Start()
	{
		StartCoroutine(TextFadeOut());
	}


	IEnumerator TextFadeOut()
	{
		yield return new WaitForSeconds(destroyTime);

		EndTextEvent?.Invoke();
		Destroy(this.gameObject);
	}
}
