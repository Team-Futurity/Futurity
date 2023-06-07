 using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
	[Header("Scene ���� �Ѱ� �޴���")]



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
		//#����#	�ڱ� �ڽ��� �ٽ� �ҷ����� �Լ�

		SceneKeyData sceneKeyData = new SceneKeyData();

		sceneKeyData.sceneName = SceneManager.GetActiveScene().name;

		SceneChangeManager.Instance.SceneLoad(sceneKeyData);
	}

	IEnumerator LoadSceneProcess()
	{
		//Scene�� �ҷ��������� Ȯ��
		while (SceneManager.GetActiveScene().name != "LoadingScene")
		{
			yield return null;
		}

		//AsyncOperation�� �񵿱� �۾��� ó���Ҷ� ����ϴ� Ŭ����
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);
		//�ε� ���� �����⵵ ���� �ε��� �Ϸ�� �� �ֱ� ������ �ε��� �� �Ǵ��� �ϴ� ����д�.
		asyncOperation.allowSceneActivation = false;


		loadingBar = GameObject.Find("LoadingBar");
		LoadingSceneController loadingController = loadingBar.GetComponent<LoadingSceneController>();
		loadingBarImage = loadingController.loadingBar;
		loadingBarImage.fillAmount = 0f;


		loadingController.SetStageNameObject(loadSceneKey.chapterName, loadSceneKey.sceneName, loadSceneKey.incidentName);


		float timer = 0f; 
		float duration = 1f; // SmoothStep ���� �ð�, �� ���� �����Ͽ� �ε巯�� �̵��� �󸶳� ������ �̷������ ����

		while (!asyncOperation.isDone)
		{
			yield return null;

			if (asyncOperation.progress < 0.9f)
			{
				timer += Time.unscaledDeltaTime / duration;
				// SmoothStep�� �̿��Ͽ� �ε� ���൵�� �ð������� �ε巴�� ǥ��
				loadingBarImage.fillAmount = Mathf.SmoothStep(loadingBarImage.fillAmount, asyncOperation.progress, timer);
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				loadingBarImage.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

				//Scene�� �ε��� ������� SceneȰ��ȭ(����)
				if (loadingBarImage.fillAmount >= 1f)
				{
					yield return FadeManager.Instance.FadeCoroutineStart(false, 1, Color.black);
					FadeManager.Instance.FadeStart(false, 0, Color.black);

					asyncOperation.allowSceneActivation = true;

					WindowManager.Instance.WindowsClearner();

					while (SceneManager.GetActiveScene().name != loadSceneName)
					{
						FadeManager.Instance.FadeStart(true, 1, Color.black);
						yield break;
					}
				}
			}

			FDebug.Log($"Loading ���൵ : {(int)(loadingBarImage.fillAmount * 100)}");
		}
	}
}