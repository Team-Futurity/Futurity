using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area3_LastKillCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector area3LastKill;
	
	[Header("스크립트 데이터")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;

	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		chapterManager.SetActivePlayerInput(false);

		chapterManager.isCutScenePlay = true;
	}

	public override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		chapterManager.SetActivePlayerInput(true);

		chapterManager.isCutScenePlay = false;
		// TODO : 3챕터 이동 트리거 발동
	}

	public void Area3_LastKillScripts()
	{
		area3LastKill.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(area3LastKill, scriptsList, curScriptsIndex);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
}
