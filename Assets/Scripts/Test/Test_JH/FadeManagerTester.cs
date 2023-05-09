using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class FadeManagerTester : MonoBehaviour
{
	[SerializeField]
	private float _waitTime = 2.0f;

	FadeManager fadeManager;

	private void Awake()
	{
		fadeManager = Singleton.Instance.fadeManager;
	}

	private void Start()
	{
		Invoke(nameof(TestFadeInOut), _waitTime);
	}

	private void TestFadeInOut()
	{
		// Fade Out
		fadeManager.FadeStart(false, 1f, Color.black);

		// ±â´Ù·È´Ù°¡ Fade In
		Invoke(nameof(TestFadeIn), _waitTime);
	}

	private void TestFadeIn()
	{
		// Fade In
		fadeManager.FadeStart(true, 1f, Color.black);

		// ±â´Ù·È´Ù°¡ Fade Out
		Invoke(nameof(TestFadeInOut), _waitTime);
	}
}