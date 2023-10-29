using UnityEngine;

[System.Serializable]
public class GUIPanelData
{
	[SerializeField, Tooltip("�ܼ��� ��� ������ �����մϴ�. true = ��, false = ����")]
	private bool isTopAttached;

	[SerializeField, Tooltip("Debug UI�� �����Դϴ�. TopAttachhed = true�� ��� Width, �ƴ� ��� Height�Դϴ�.")]
	private float length;

	[SerializeField, Tooltip("Length�� �ݴ� �����Դϴ�. Length�� Width�� ��� Height, Height�� ��� Width�� ���˴ϴ�.")]
	private float otherLength;

	[SerializeField, Tooltip("Debug UI ���̸� ������� �����մϴ�\nTrue�� �����ϸ� ȭ�� ���̿��� Length�� ���ϴ�")]
	private bool isReduceMode;

	public Vector2 GetGUIPanelSize()
	{
		float ratioW = Screen.width / 1920;
		float ratioH = Screen.height / 1080;

		float uiWidth = isTopAttached ? GetLength() : otherLength * ratioW;
		float uiHeight = isTopAttached ? otherLength * ratioH : GetLength();

		return new Vector2(uiWidth, uiHeight);
	}

	// Length�� ��ȯ�մϴ�.
	private float GetLength()
	{
		float ratioW = Screen.width / 1920;
		float ratioH = Screen.height / 1080;

		float screenLength = isTopAttached ? Screen.width : Screen.height;
		float dir = isTopAttached ? ratioW : ratioH;
		float result = isReduceMode ? screenLength - length * dir : length * dir;

		return result;
	}

}
