using UnityEngine;

[System.Serializable]
public class GUIPanelData
{
	[SerializeField, Tooltip("콘솔을 어디에 붙일지 결정합니다. true = 위, false = 왼쪽")]
	private bool isTopAttached;

	[SerializeField, Tooltip("Debug UI의 길이입니다. TopAttachhed = true일 경우 Width, 아닐 경우 Height입니다.")]
	private float length;

	[SerializeField, Tooltip("Length의 반대 길이입니다. Length가 Width일 경우 Height, Height일 경우 Width로 사용됩니다.")]
	private float otherLength;

	[SerializeField, Tooltip("Debug UI 길이를 감산모드로 설정합니다\nTrue로 변경하면 화면 길이에서 Length를 뺍니다")]
	private bool isReduceMode;

	public Vector2 GetGUIPanelSize()
	{
		float ratioW = Screen.width / 1920;
		float ratioH = Screen.height / 1080;

		float uiWidth = isTopAttached ? GetLength() : otherLength * ratioW;
		float uiHeight = isTopAttached ? otherLength * ratioH : GetLength();

		return new Vector2(uiWidth, uiHeight);
	}

	// Length를 반환합니다.
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
