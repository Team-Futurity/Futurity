using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScrollingCredits : MonoBehaviour
{
	public float speed = 50f; // 크레딧이 스크롤되는 속도, 필요에 따라 조절해주세요.
	public GameObject textPrefab; // TextMeshPro 텍스트 프리팹.
	public List<string> credits = new List<string>(); // 크레딧 리스트.
	public Transform startPoint; // 텍스트 시작 위치.
	public float delay = 1f; // 크레딧 간의 딜레이 시간.
	public UnityEvent OnCreditsEnd; // 모든 크레딧이 스크롤 된 후에 호출될 이벤트
	public float cutOffPoint = 600f; // 크레딧이 화면에서 사라져야 하는 y 좌표
	private int creditsCount; // 생성된 크레딧의 총 개수

	void Start()
	{
		creditsCount = credits.Count;
		StartCoroutine(CreateCredits());
	}

	IEnumerator CreateCredits()
	{
		foreach (string credit in credits)
		{
			GameObject textObject = Instantiate(textPrefab, startPoint.position, Quaternion.identity, startPoint);
			textObject.transform.SetAsLastSibling(); // 리스트의 마지막 요소가 가장 위에 나타나도록 합니다.
			textObject.GetComponent<TextMeshProUGUI>().text = credit;
			yield return new WaitForSeconds(delay); // 다음 크레딧이 등장하기 전에 지정된 딜레이를 적용합니다.
		}
	}

	void Update()
	{
		// 모든 텍스트 객체를 스크롤.
		for (int i = startPoint.childCount - 1; i >= 0; i--)
		{
			Transform child = startPoint.GetChild(i);
			child.Translate(Vector3.up * speed * Time.deltaTime);
			if (child.localPosition.y > cutOffPoint)
			{
				creditsCount--;
				Destroy(child.gameObject);
			}
		}

		// 모든 크레딧이 화면에서 사라졌을 때 이벤트를 호출합니다.
		if (creditsCount <= 0)
		{
			OnCreditsEnd.Invoke();
			creditsCount = credits.Count; // Reset the count in case the credits play again
		}
	}
}
