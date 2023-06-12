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
	private float loadingBarSpeed = 1;
	[SerializeField]
	private float loadingDelayTime = 0.25f;
	WaitForSeconds loadingWaitForSeconds;

	[SerializeField]
	private SceneKeyData loadSceneKey;

	public void Start()
	{
		loadingWaitForSeconds = new WaitForSeconds(loadingDelayTime);
	}

	public void SceneLoad(SceneKeyData loadSceneKey, int loadingSceneNumber)
	{
		


		this.loadSceneKey = loadSceneKey;
		loadSceneName = loadSceneKey.sceneName;
		SceneManager.LoadScene($"LoadingScene {loadingSceneNumber}");
		StartCoroutine(LoadSceneProcess(loadingSceneNumber));
	}

	public void SelfSceneLoad()
	{
		//#����#	�ڱ� �ڽ��� �ٽ� �ҷ����� �Լ�

		SceneKeyData sceneKeyData = new SceneKeyData();

		sceneKeyData.sceneName = SceneManager.GetActiveScene().name;
		sceneKeyData.chapterName = "";
		sceneKeyData.incidentName = "";

		SceneChangeManager.Instance.SceneLoad(sceneKeyData, 1);
	}

	IEnumerator LoadSceneProcess(int loadingSceneNumber)
	{
		yield return FadeManager.Instance.FadeCoroutineStart(true, 1, Color.black);

		//Scene�� �ҷ��������� Ȯ��
		while (SceneManager.GetActiveScene().name != $"LoadingScene {loadingSceneNumber}")
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


		yield return loadingWaitForSeconds;

		while (!asyncOperation.isDone)
		{
			yield return null;

			if (loadingBarImage.fillAmount < 0.89f)
			{
				timer += Time.unscaledDeltaTime / duration;
				// SmoothStep�� �̿��Ͽ� �ε� ���൵�� �ð������� �ε巴�� ǥ��
				loadingBarImage.fillAmount = Mathf.SmoothStep(loadingBarImage.fillAmount, asyncOperation.progress, timer / loadingBarSpeed);
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				loadingBarImage.fillAmount = Mathf.Lerp(0.9f, 1f, timer / loadingBarSpeed);


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