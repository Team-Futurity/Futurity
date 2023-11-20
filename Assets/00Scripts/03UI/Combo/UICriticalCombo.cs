using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UICriticalCombo : MonoBehaviour
{
	enum AnimationType
	{
		CRITICAL,
		COUNT
	}

	// Number Image & Group
	public Image[] numberImages;
	public RectTransform numberGroup;

	public float animSpeed = .0f;
	public float activeTime = .0f;

	// Critical Image
	public Image criticalImages;

	// Sprite Data Base
	public Sprite[] numberSpriteImages;

	private Vector2 currentLocalPos;
	public Vector2 animStartPos;

	// Viewer
	[SerializeField, Header("콤보 시스템"), Space(10)]
	private HitCountSystem countSystem;

	#region Index
	private const int Number100		= 0;
	private const int Number10		= 1;
	private const int Number1		= 2;
	#endregion

	private void Awake()
	{
		// End Pos 지정
		currentLocalPos = numberGroup.anchoredPosition;

		// Number Data Load
		// LoadNumberSpriteImages();

		// Number Init
		InitNumbers();

		// Event 등록하기
		countSystem.updateHitCount?.AddListener(UpdateNumberImages);

		numberGroup.gameObject.SetActive(false);

	}

	private void InitNumbers()
	{
		for(int i = 0;i<  numberImages.Length; ++i)
		{
			// 모든 수를 0으로 초기화.
			numberImages[i].sprite = numberSpriteImages[0];
		}
	}

	// 해당 메서드를 통해서 Combo Image를 Update 한다.
	public void UpdateNumberImages(int comboCount)
	{
		numberGroup.gameObject.SetActive(true);

		if (comboCount > 999) comboCount = 999;
		if (comboCount < 0) comboCount = 0;

		// 100
		SetNumberImage(Number100, comboCount / 100);
		// 10
		SetNumberImage(Number10, (comboCount / 10) % 10);
		// 1
		SetNumberImage(Number1, comboCount % 10);

		// Animation을 실행한다.
		PlayAnimation(AnimationType.COUNT);
	}

	// 해당 메서드를 통해서 Critical Image를 띄운다.
	public void SetCriticalImage()
	{
		PlayAnimation(AnimationType.CRITICAL);
	}

	// 해당 인덱스에 숫자로 변경한다.
	private void SetNumberImage(int index, int indexCount)
	{
		var spriteName = numberImages[index].sprite.name;
		var indexName = indexCount.ToString();

		if (spriteName.Equals(indexName))
		{
			return;
		}

		numberImages[index].sprite = numberSpriteImages[indexCount];
	}

	// 몇 초 뒤에 끈다.
	private IEnumerator OffNumberImage()
	{
		var timer = .0f;

		while(timer < activeTime)
		{
			yield return null;

			timer += Time.deltaTime;
		}

		numberGroup.gameObject.SetActive(false);
	}

	// 몇 초 뒤에 끈다.
	private void OffCriticalImage()
	{

	}

	private void PlayAnimation(AnimationType type)
	{
		switch(type)
		{
			case AnimationType.CRITICAL:
				break;

			case AnimationType.COUNT:
				StartCoroutine("PlayCountAnimation");
				break;

			default:
				break;
		}
	}

	private IEnumerator PlayCountAnimation()
	{
		StopCoroutine("OffNumberImage");
		float timer = .0f;

		while (timer < 1f)
		{
			timer += Time.deltaTime * animSpeed;

			if(timer > 1f) { timer = 1.0f; }

			var resultPosX = EaseInExpo(animStartPos.x, currentLocalPos.x, timer);
			var resultPosY = EaseInExpo(animStartPos.y, currentLocalPos.y, timer);

			yield return null;

			numberGroup.anchoredPosition = new Vector2(resultPosX, resultPosY);
		}

		StartCoroutine("OffNumberImage");
	}

	public static float EaseInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2, 10 * (value - 1)) + start;
	}
}
