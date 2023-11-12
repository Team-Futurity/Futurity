using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToTitleScene : MonoBehaviour
{
	private void Update()
	{
		SceneManager.LoadScene("TitleScene");
		Destroy(gameObject);
	}
}
