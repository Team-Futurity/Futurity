Shader "Custom/Grayscale"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {} // �⺻ �ؽ�ó�� �����ϰ� �ʱⰪ�� "���"���� ����
		_Grayscale("Grayscale", Range(0.0, 1.0)) = 0.0 // ��� ó���� ������ �����ϴ� ������ �����ϰ� �ʱⰪ�� 0���� ����
	}

		SubShader
		{
			Pass
			{
				CGPROGRAM // ���̴� ���α׷��� ����
				#pragma vertex vert_img // �̹����� ����Ǵ� ���� ���̴��� ����
				#pragma fragment frag // �����׸�Ʈ ���̴��� ����
				#include "UnityCG.cginc" // Unity�� ���� ���̴� ���̺귯���� ����

				sampler2D _MainTex; // _MainTex �Ӽ��� ���� ���÷��� ����
				float _Grayscale; // _Grayscale �Ӽ��� ���� ������ ����

				// �� �ȼ��� ���� ������ ����ϴ� �����׸�Ʈ ���̴� �Լ�
				fixed4 frag(v2f_img i) : COLOR
				{
					fixed4 currentText = tex2D(_MainTex, i.uv); // _MainTex �ؽ�ó���� �ȼ��� ���� ������ ������

				// ��� ��(grayscale)�� ���. �� ���� ���� �ȼ��� RGB ä�� ���� ���
				float grayscale = (currentText.r + currentText.g + currentText.b) / 3;

				// grayscale�� �̿��Ͽ� ���� ����� ��� ���� ���̸� _Grayscale ���� ���� ����
				fixed4 color = lerp(currentText, grayscale, _Grayscale);

				// ������ ������ ����Ͽ� ���� �ȼ��� ������ ����
				currentText.rgb = color;

				// ���������� ���� �ȼ��� ������ ��ȯ
				return currentText;
			}

		ENDCG // ���̴� ���α׷��� ����
		}
		}
			FallBack off // ���� ���̴��� ����� �� ���� �� ��ü�� ���̴��� �������� ����
}
