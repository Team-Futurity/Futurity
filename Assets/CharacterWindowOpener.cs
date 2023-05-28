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

		controller.SetCharactorText("… … 미래 씨! ");
		controller.SetCharactorText("제 목소리 들려요?");
		controller.SetCharactorText("크로마 자기장 영역 때문에 더 이상 접근하는 건 무리에요…!");
		controller.SetCharactorText("힘들어도 여기서부터는 도보 이동입니다!");
		controller.SetCharactorText("어디 보자…");
		controller.SetCharactorText("로데오 거리, 번화가에서 거대한 전기 신호가 수집되었어요!");
		controller.SetCharactorText("일단 백화점을 벗어나 지하철역으로 진입해 주세요.");
		controller.SetCharactorText("그전까지 제가 지하철의 통신망을 해킹하고 있을게요.");
		controller.SetCharactorText("… 다치지 말라는 말은 하지 않겠지만, 조심해야 해요…!");
	}
}
