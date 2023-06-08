using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraGameOverEffect : MonoBehaviour
{
	Material cameraMaterial;
	public float grayScale = 0.0f;

	public Text message;
	float appliedTime = 2.0f;

	void Start()
	{
		message.enabled = false;
		cameraMaterial = new Material(Shader.Find("Custom/Grayscale"));
	}

	//��ó�� ȿ��. src �̹���(���� ȭ��)�� dest �̹����� ��ü
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		cameraMaterial.SetFloat("_Grayscale", grayScale);
		Graphics.Blit(src, dest, cameraMaterial);
	}

	public void gameOverCameraEffect()
	{
		StartCoroutine(gameOverEffect());
	}

	private IEnumerator gameOverEffect()
	{
		float elapsedTime = 0.0f;

		while (elapsedTime < appliedTime)
		{
			elapsedTime += Time.deltaTime;

			grayScale = elapsedTime / appliedTime;
			yield return null;
		}

		message.text = "GAME OVER!";
		message.enabled = true;
	}
}