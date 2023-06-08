using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScrollingCredits : MonoBehaviour
{
	public float speed = 50f; // ũ������ ��ũ�ѵǴ� �ӵ�, �ʿ信 ���� �������ּ���.
	public GameObject textPrefab; // TextMeshPro �ؽ�Ʈ ������.
	public List<string> credits = new List<string>(); // ũ���� ����Ʈ.
	public Transform startPoint; // �ؽ�Ʈ ���� ��ġ.
	public float delay = 1f; // ũ���� ���� ������ �ð�.
	public UnityEvent OnCreditsEnd; // ��� ũ������ ��ũ�� �� �Ŀ� ȣ��� �̺�Ʈ
	public float cutOffPoint = 600f; // ũ������ ȭ�鿡�� ������� �ϴ� y ��ǥ
	private int creditsCount; // ������ ũ������ �� ����

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
			textObject.transform.SetAsLastSibling(); // ����Ʈ�� ������ ��Ұ� ���� ���� ��Ÿ������ �մϴ�.
			textObject.GetComponent<TextMeshProUGUI>().text = credit;
			yield return new WaitForSeconds(delay); // ���� ũ������ �����ϱ� ���� ������ �����̸� �����մϴ�.
		}
	}

	void Update()
	{
		// ��� �ؽ�Ʈ ��ü�� ��ũ��.
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

		// ��� ũ������ ȭ�鿡�� ������� �� �̺�Ʈ�� ȣ���մϴ�.
		if (creditsCount <= 0)
		{
			OnCreditsEnd.Invoke();
			creditsCount = credits.Count; // Reset the count in case the credits play again
		}
	}
}
