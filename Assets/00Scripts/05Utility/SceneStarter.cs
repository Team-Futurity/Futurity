using UnityEngine;

public class SceneStarter : MonoBehaviour
{
	private void Start()
	{
		ChapterMoveController.Instance.OnEnableController();
	}
}
