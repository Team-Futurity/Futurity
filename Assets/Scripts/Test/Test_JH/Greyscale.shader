Shader "Custom/Grayscale"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {} // 기본 텍스처를 선언하고 초기값을 "흰색"으로 설정
		_Grayscale("Grayscale", Range(0.0, 1.0)) = 0.0 // 흑백 처리의 정도를 조절하는 변수를 선언하고 초기값을 0으로 설정
	}

		SubShader
		{
			Pass
			{
				CGPROGRAM // 쉐이더 프로그램을 시작
				#pragma vertex vert_img // 이미지에 적용되는 정점 쉐이더를 지정
				#pragma fragment frag // 프래그먼트 쉐이더를 지정
				#include "UnityCG.cginc" // Unity의 내장 쉐이더 라이브러리를 포함

				sampler2D _MainTex; // _MainTex 속성에 대한 샘플러를 선언
				float _Grayscale; // _Grayscale 속성에 대한 변수를 선언

				// 각 픽셀의 최종 색상을 계산하는 프래그먼트 쉐이더 함수
				fixed4 frag(v2f_img i) : COLOR
				{
					fixed4 currentText = tex2D(_MainTex, i.uv); // _MainTex 텍스처에서 픽셀의 현재 색상을 가져옴

				// 흑백 값(grayscale)을 계산. 이 값은 현재 픽셀의 RGB 채널 값의 평균
				float grayscale = (currentText.r + currentText.g + currentText.b) / 3;

				// grayscale을 이용하여 현재 색상과 흑백 색상 사이를 _Grayscale 값에 따라 보간
				fixed4 color = lerp(currentText, grayscale, _Grayscale);

				// 보간된 색상을 사용하여 현재 픽셀의 색상을 설정
				currentText.rgb = color;

				// 최종적으로 계산된 픽셀의 색상을 반환
				return currentText;
			}

		ENDCG // 쉐이더 프로그램을 종료
		}
		}
			FallBack off // 현재 쉐이더가 사용할 수 없을 때 대체할 쉐이더를 지정하지 않음
}
