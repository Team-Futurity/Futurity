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
		float uiWidth = isTopAttached ? GetLength() : otherLength;
		float uiHeight = isTopAttached ? otherLength : GetLength();

		return new Vector2(uiWidth, uiHeight);
	}

	// Length�� ��ȯ�մϴ�.
	private float GetLength()
	{
		float screenLength = isTopAttached ? Screen.width : Screen.height;
		float result = isReduceMode ? screenLength - length : length;

		return result;
	}

}
