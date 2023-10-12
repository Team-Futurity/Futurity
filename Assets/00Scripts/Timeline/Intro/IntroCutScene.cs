using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector introCurScene;
	[SerializeField] private TextMeshProUGUI inputField;
	
	[Header("스크립트 데이터")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;
	
	[Header("Fade Time")] 
	[SerializeField] private float fadeInTime = 0.8f;
	[SerializeField] private float fadeOutTime = 0.8f;

	[Header("다음으로 이동할 씬 이름")] 
	[SerializeField] private string nextSceneName;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		FadeManager.Instance.FadeOut(fadeInTime);
	}

	public override void DisableCutScene()
	{
		FadeManager.Instance.FadeIn(fadeOutTime, () =>
		{
			SceneLoader.Instance.LoadScene(nextSceneName);
		});
	}
	
	public void Intro_PrintScripts()
	{
		introCurScene.Pause();

		chapterManager.PauseCutSceneUntilScriptsEnd(introCurScene, scriptsList, curScriptsIndex);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
	
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}

	public void ClearInputField() => inputField.text = "";
}
