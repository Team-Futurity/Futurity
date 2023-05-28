using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterWindowOpener : MonoBehaviour
{
	[SerializeField]
	Canvas canvas;

	[SerializeField]
	GameObject characterWindow;

	GameObject currentCharacterWindow;

	private void Start()
	{
		currentCharacterWindow = WindowManager.Instance.WindowOpen(characterWindow, canvas.transform, false, Vector2.zero, Vector2.one);

		CharacterDialogController controller = currentCharacterWindow.GetComponent<CharacterDialogController>();

		controller.SetCharactorText("�� �� �̷� ��! ");
		controller.SetCharactorText("�� ��Ҹ� �����?");
		controller.SetCharactorText("ũ�θ� �ڱ��� ���� ������ �� �̻� �����ϴ� �� �������䡦!");
		controller.SetCharactorText("���� ���⼭���ʹ� ���� �̵��Դϴ�!");
		controller.SetCharactorText("��� ���ڡ�");
		controller.SetCharactorText("�ε��� �Ÿ�, ��ȭ������ �Ŵ��� ���� ��ȣ�� �����Ǿ����!");
		controller.SetCharactorText("�ϴ� ��ȭ���� ��� ����ö������ ������ �ּ���.");
		controller.SetCharactorText("�������� ���� ����ö�� ��Ÿ��� ��ŷ�ϰ� �����Կ�.");
		controller.SetCharactorText("�� ��ġ�� ����� ���� ���� �ʰ�����, �����ؾ� �ؿ䡦!");
	}
}
