using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	[SerializeField]
	private string loadSceneName;

	[SerializeField]
	private Image loadingBar;

	public void SceneLoader(string loadSceneName)
	{
		this.loadSceneName = loadSceneName;
		SceneManager.LoadScene("LoadingScene");
		StartCoroutine(LoadSceneProcess());
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


		loadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();

		float timer = 0f;
		while (!asyncOperation.isDone)
		{
			yield return null;

			if (asyncOperation.progress < 0.9f)
			{
				loadingBar.fillAmount = asyncOperation.progress;
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

				//Scene�� �ε��� ������� SceneȰ��ȭ(����)
				if (loadingBar.fillAmount >= 1f)
				{
					asyncOperation.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
}