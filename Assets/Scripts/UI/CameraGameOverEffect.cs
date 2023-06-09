using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraGameOverEffect : MonoBehaviour
{
	[SerializeField]
	Material cameraMaterial; // ������ ���̴� ���͸���
	public float grayScale = 0.0f; // ��� ��ȯ ����. 0.0(���� ����) ~ 1.0(���� ���)

	float appliedTime = 2.0f; // ��� ��ȯ�� �ɸ��� �ð�

	void Start()
	{
		// "Custom/Grayscale" ���̴��� ã�� Material�� �����Ͽ� ����
		cameraMaterial = new Material(Shader.Find("Custom/Grayscale"));
	}

	//��ó�� ȿ��. src �̹���(���� ȭ��)�� dest �̹����� ��ü
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		// grayscale ���� ���̴��� ����
		cameraMaterial.SetFloat("_Grayscale", grayScale);

		// src �ؽ�ó�� dest �ؽ�ó�� �������ϵ�, ����� ���̴��� cameraMaterial
		Graphics.Blit(src, dest, cameraMaterial);
	}

	// ���� ���� ȿ�� ������ ���� public �޼���
	public void gameOverCameraEffect()
	{
		// �ڷ�ƾ�� �����Ͽ� ���������� ȭ���� ������� �ٲٴ� �۾� ����
		StartCoroutine(gameOverEffect());
	}

	// ���� ���� ȿ���� ���� �ڷ�ƾ
	private IEnumerator gameOverEffect()
	{
		float elapsedTime = 0.0f; // �帥 �ð�

		// elapsedTime�� appliedTime���� ���� ���� �ݺ�
		while (elapsedTime < appliedTime)
		{
			// �ð� ����
			elapsedTime += Time.deltaTime;

			// grayScale ���� elapsedTime�� appliedTime�� ������ ����. �̴� ���������� ������� �ٲٴ� ���� �ǹ�
			grayScale = elapsedTime / appliedTime;

			yield return null; // ���� �����ӱ��� ���
		}
	}
}
