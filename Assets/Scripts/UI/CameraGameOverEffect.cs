using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraGameOverEffect : MonoBehaviour
{
	[SerializeField]
	Material cameraMaterial; // 적용할 쉐이더 메터리얼
	public float grayScale = 0.0f; // 흑백 변환 비율. 0.0(원래 색상) ~ 1.0(완전 흑백)

	float appliedTime = 2.0f; // 흑백 변환에 걸리는 시간

	void Start()
	{
		// "Custom/Grayscale" 쉐이더를 찾아 Material로 생성하여 저장
		cameraMaterial = new Material(Shader.Find("Custom/Grayscale"));
	}

	//후처리 효과. src 이미지(현재 화면)를 dest 이미지로 교체
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		// grayscale 값을 쉐이더에 전달
		cameraMaterial.SetFloat("_Grayscale", grayScale);

		// src 텍스처를 dest 텍스처에 렌더링하되, 사용할 쉐이더는 cameraMaterial
		Graphics.Blit(src, dest, cameraMaterial);
	}

	// 게임 오버 효과 시작을 위한 public 메서드
	public void gameOverCameraEffect()
	{
		// 코루틴을 시작하여 점차적으로 화면을 흑백으로 바꾸는 작업 수행
		StartCoroutine(gameOverEffect());
	}

	// 게임 오버 효과를 위한 코루틴
	private IEnumerator gameOverEffect()
	{
		float elapsedTime = 0.0f; // 흐른 시간

		// elapsedTime가 appliedTime보다 작은 동안 반복
		while (elapsedTime < appliedTime)
		{
			// 시간 증가
			elapsedTime += Time.deltaTime;

			// grayScale 값을 elapsedTime와 appliedTime의 비율로 설정. 이는 점차적으로 흑백으로 바꾸는 것을 의미
			grayScale = elapsedTime / appliedTime;

			yield return null; // 다음 프레임까지 대기
		}
	}
}
