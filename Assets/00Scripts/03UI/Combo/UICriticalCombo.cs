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
	[SerializeField, Header("�޺� �ý���"), Space(10)]
	private HitCountSystem countSystem;

	#region Index
	private const int Number100		= 0;
	private const int Number10		= 1;
	private const int Number1		= 2;
	#endregion

	private void Awake()
	{
		// End Pos ����
		currentLocalPos = numberGroup.anchoredPosition;

		// Number Data Load
		// LoadNumberSpriteImages();

		// Number Init
		InitNumbers();

		// Event ����ϱ�
		countSystem.updateHitCount?.AddListener(UpdateNumberImages);

		numberGroup.gameObject.SetActive(false);

	}

	private void InitNumbers()
	{
		for(int i = 0;i<  numberImages.Length; ++i)
		{
			// ��� ���� 0���� �ʱ�ȭ.
			numberImages[i].sprite = numberSpriteImages[0];
		}
	}

	// �ش� �޼��带 ���ؼ� Combo Image�� Update �Ѵ�.
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

		// Animation�� �����Ѵ�.
		PlayAnimation(AnimationType.COUNT);
	}

	// �ش� �޼��带 ���ؼ� Critical Image�� ����.
	public void SetCriticalImage()
	{
		PlayAnimation(AnimationType.CRITICAL);
	}

	// �ش� �ε����� ���ڷ� �����Ѵ�.
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

	// �� �� �ڿ� ����.
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

	// �� �� �ڿ� ����.
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
