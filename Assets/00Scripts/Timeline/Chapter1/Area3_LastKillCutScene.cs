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
	
	private TimelineManager manager = null;
	private int curScriptsIndex = 0;

	protected override void Init()
	{
		manager = TimelineManager.Instance;
	}

	protected override void EnableCutScene()
	{
		manager.SetActiveMainUI(false);
		manager.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		manager.SetActiveMainUI(true);
		manager.SetActivePlayerInput(true);
		
		// TODO : 3챕터 이동 트리거 발동
	}

	public void Area3_LastKillScripts()
	{
		area3LastKill.Pause();
		
		manager.PauseCutSceneUntilScriptsEnd(area3LastKill, scriptsList, curScriptsIndex);
		manager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
}
