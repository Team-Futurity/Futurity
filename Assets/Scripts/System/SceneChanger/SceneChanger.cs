using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
	[SerializeField]
	private string loadSceneName;

	[SerializeField]
	private Image loadingBar;

	public void SceneLoader(string loadSceneName)
	{
		this.loadSceneName = loadSceneName;
		SceneManager.LoadScene("LoadingScene");
		Debug.Log($"SceneManager.GetActiveScene().name : {SceneManager.GetActiveScene().name}");
		StartCoroutine(LoadSceneProcess());
	}

	IEnumerator LoadSceneProcess()
	{
		Debug.Log($"SceneManager.GetActiveScene().name : {SceneManager.GetActiveScene().name}");

		//Scene이 불러와졌는지 확인
		while (SceneManager.GetActiveScene().name != "LoadingScene")
		{
			yield return null;
		}

		//AsyncOperation은 비동기 작업을 처리할때 사용하는 클래스
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);
		//로딩 씬이 나오기도 전에 로딩이 완료될 수 있기 때문에 로딩이 다 되더라도 일단 멈춰둔다.
		asyncOperation.allowSceneActivation = false;

		Debug.Log($"SceneManager.GetActiveScene().name : {SceneManager.GetActiveScene().name}");

		loadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();
		Debug.Log($"LoadingBar : {loadingBar}");

		float timer = 0f;
		while (!asyncOperation.isDone)
		{
			yield return null;

			if (asyncOperation.progress < 0.85f)
			{
				loadingBar.fillAmount = asyncOperation.progress;
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				loadingBar.fillAmount = Mathf.Lerp(0.85f, 1f, timer);

				//Scene의 로딩이 끝날경우 Scene활성화(번경)
				if (loadingBar.fillAmount >= 1f)
				{
					asyncOperation.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
}