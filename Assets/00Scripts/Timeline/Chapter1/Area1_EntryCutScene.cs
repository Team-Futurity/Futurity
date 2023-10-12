using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Area1_EntryCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private GameObject playerCamera;
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private PlayableDirector entryCutScene;

	[Header("진입 컷신에서 활성화할 오브젝트 목록")]
	[SerializeField] private GameObject[] walls;
	[SerializeField] private GameObject eliteEnemy;

	[Header("플레이어 이동값")] 
	[SerializeField] private GameObject targetPos;
	[SerializeField] private float duration;

	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex;

	protected override void Init()
	{
		chapterManager.SetActiveMainUI(false);
		chapterManager.SetActivePlayerInput(false);
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
	}
	
	public override void DisableCutScene()
	{
		foreach (var wall in walls)
		{
			wall.SetActive(true);
		}
		
		playerCamera.SetActive(true);
		eliteEnemy.SetActive(false);
		
		chapterManager.SetActiveMainUI(true);
		chapterManager.SetActivePlayerInput(true);
		chapterManager.isCutScenePlay = false;
		
		chapterManager.PlayerController.playerData.status.updateHPEvent
			.Invoke(230f, 230f);
	}

	public void Area1_Scripting()
	{
		entryCutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(entryCutScene, scriptsList, curScriptsIndex);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
	
	public void MovePlayer()
	{
		chapterManager.PlayerController.LerpToWorldPosition(targetPos.transform.position, duration);
	}

	public void Area1_SpawnEnemy()
	{
		spawnerManager.SpawnEnemy();
	}
}
