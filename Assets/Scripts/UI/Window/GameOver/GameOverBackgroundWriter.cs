using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverBackgroundWriter : MonoBehaviour
{
	[Header ("게임 오버시 벡그라운드에 출력되는 이미지를 담당하는 클래스")]
	[Space (20)]


    public List<Sprite> writeSprites;
	public int writeValue;
	public float writeDelay;
	public float writeTime;

	private RectTransform rectTransform;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		StartCoroutine(SpriteWriter());
	}

	private IEnumerator SpriteWriter()
	{
		for (int i = 0; i < writeValue; i++)
		{
			if (writeSprites.Count > 0)
			{
				Sprite randomSprite = writeSprites[Random.Range(0, writeSprites.Count)];

				GameObject newSpriteObject = new GameObject("GeneratedSprite");
				newSpriteObject.transform.SetParent(rectTransform, false);

				Image image = newSpriteObject.AddComponent<Image>();
				image.sprite = randomSprite;

				newSpriteObject.transform.localPosition = new Vector3(
					Random.Range(rectTransform.rect.xMin, rectTransform.rect.xMax),
					Random.Range(rectTransform.rect.yMin, rectTransform.rect.yMax),
					0);

				StartCoroutine(ExpandSprite(image));
			}

			if (i < writeValue - 1)
			{
				yield return new WaitForSeconds(writeDelay);
			}
		}
	}

	private IEnumerator ExpandSprite(Image spriteImage)
	{
		float timer = 0f;

		while (timer < writeTime)
		{
			timer += Time.deltaTime;
			float size = Mathf.Lerp(0, 1, timer / writeTime);
			spriteImage.rectTransform.localScale = new Vector3(size, size, size);
			yield return null;
		}
	}
}
