 using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
	[Header("Scene 변경 총괄 메니저")]



	[SerializeField]
	private string loadSceneName;

	[SerializeField]
	private Image loadingBarImage;
	[SerializeField]
	private GameObject loadingBar;

	[SerializeField]
	private SceneKeyData loadSceneKey;

	public void SceneLoad(SceneKeyData loadSceneKey)
	{
		this.loadSceneKey = loadSceneKey;
		loadSceneName = loadSceneKey.sceneName;
		SceneManager.LoadScene("LoadingScene");
		StartCoroutine(LoadSceneProcess());
	}

	public void SelfSceneLoad()
	{
		//#설명#	자기 자신을 다시 불러오는 함수

		SceneKeyData sceneKeyData = new SceneKeyData();

		sceneKeyData.sceneName = SceneManager.GetActiveScene().name;

		SceneChangeManager.Instance.SceneLoad(sceneKeyData);
	}

	IEnumerator LoadSceneProcess()
	{
		//Scene이 불러와졌는지 확인
		while (SceneManager.GetActiveScene().name != "LoadingScene")
		{
			yield return null;
		}

		//AsyncOperation은 비동기 작업을 처리할때 사용하는 클래스
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);
		//로딩 씬이 나오기도 전에 로딩이 완료될 수 있기 때문에 로딩이 다 되더라도 일단 멈춰둔다.
		asyncOperation.allowSceneActivation = false;


		loadingBar = GameObject.Find("LoadingBar");
		LoadingSceneController loadingController = loadingBar.GetComponent<LoadingSceneController>();
		loadingBarImage = loadingController.loadingBar;


		loadingController.SetStageNameObject(loadSceneKey.chapterName, loadSceneKey.sceneName, loadSceneKey.incidentName);


		float timer = 0f;
		while (!asyncOperation.isDone)
		{
			yield return null;

			if (asyncOperation.progress < 0.9f)
			{
				loadingBarImage.fillAmount = asyncOperation.progress;
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				loadingBarImage.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

				//Scene의 로딩이 끝날경우 Scene활성화(번경)
				if (loadingBarImage.fillAmount >= 1f)
				{
					yield return FadeManager.Instance.FadeCoroutineStart(false, 1, Color.black);

					asyncOperation.allowSceneActivation = true;

					WindowManager.Instance.WindowsClearner();

					FadeManager.Instance.FadeStart(true, 1, Color.black);
					yield break;
				}
			}

			FDebug.Log($"Loading 진행도 : {(int)(loadingBarImage.fillAmount * 100)}");
		}
	}
}