using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSystem : MonoBehaviour
{
	[SerializeField, Header("�� Fade �ð�")]
	private float fadeTime = 1f;

	[SerializeField, Header("�ε� ������")]
	private LoadingIconMove loadIcon;

	private string nextScene = "";

	public Image img;
	public TMP_Text cctvText;
	public TMP_Text newsText;

	private List<string> zz = new List<string>()
	{
		"[�Ӻ�] ��Ʈ�� �����ҿ��� �ǹ��� ������ ����, ��� ���� ���",
		"�¿��� ��ȭ�� �ֺ����� ������ ����",
		"��Ʈ�� ���������� ��� ���� �߾� ���� '�ָ�'",
		"�����, ���� ������ ����",
		"�¿��� ȥ�� ��Ȳ�� ���� '����� ����'",
		"�¿��� ���� ���� '����'... �������� �ݹ� �ż���",
		"'������ �� ��� �Ұ��ϴ�'... �̾����� '�߸� ����'",
		"�������� �̾����� ������ �ձ�... �̱� '�� ���� ����'",
		"�ǹ��� ���� ������ ���� �¿��� '����ȭ'",
		"�¿��� ����� �ǹ��� ���� ����? �������� �ǰ� 'ȯ�� ȿ�� Ȯ��'",
		"�¿��� ����... ���Ÿ�� ���ƴ�? 12�ð��� ���� ������ �¿��ô�...",
		"�����, ������ ��ǻ� ��� �Ұ��ϴٰ� ����",
		"���� ������ ��Ʈ�� �������� '������ ���� ����'?",
		"�� �ǿ��� '���� �κ��� �����̴�.' �߾� �ݹ� ����",
		"���� �ִ� ������� ����ΰ� ��Ȳ... ������ '�̱ۺ���'",
		"���Ӱ� ������ ������ ���� ������ ��ī, �¿��÷� ������!",
		"''K �������� K��Ʈ�ο� �Բ�!' ��Ʈ�μ� ���ο� ������ �߰���",
		"���� ��¿� �ùε� '�㸮 �ش�' �� �ǿ� '��å �����ϰڴ�.'",
		"�б�, ����, ������... �� ���� �� ��Ҵ�! '�¿����� ���� ����'",
		"���Ӱ� �������� ���� ���� 'ǻó��Ƽ'... ���� ������?",
	};

	public void SetLoadData(LoadingData data)
	{
		if (data == null) return;

		img.sprite = data.cctveImage;
		cctvText.text = data.cctvText;
		newsText.text = zz[Random.Range(0, zz.Count)];
	}

	public void SetNextScene(string sceneName)
	{
		nextScene = sceneName;

		UIManager.Instance.RemoveAllWindow();

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			AudioManager.Instance.CleanUp();

			SceneLoader.Instance.updateProgress?.AddListener(loadIcon.MoveIcon);
			SceneLoader.Instance.LoadSceneAsync(nextScene);
		});
	}
}
