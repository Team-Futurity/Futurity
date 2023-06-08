using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CraditEndTextController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textMeshProUGUI;

	[SerializeField]
	private UnityEvent EndTextEvent = null;

	private float elapsedTime = 0;
	private float _fadeTime = 1.0f;

	[SerializeField]
	Color textColor = Color.white;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(TextFadeOut());
	}


	IEnumerator TextFadeOut()
	{
		elapsedTime = 0;

		while (elapsedTime < _fadeTime)
		{
			elapsedTime += Time.deltaTime;
			textColor = new Color(Mathf.Lerp(1, 0, elapsedTime / _fadeTime), textColor.g, textColor.b, Mathf.Lerp(1, 0, elapsedTime / _fadeTime));
			textMeshProUGUI.color = textColor;

			yield return null;
		}

		yield return new WaitForSeconds(1);

		EndTextEvent?.Invoke();
	}
}
