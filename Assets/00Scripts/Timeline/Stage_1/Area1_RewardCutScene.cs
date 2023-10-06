using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area1_RewardCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector rewardCutScene;
	[SerializeField] private List<ScriptingList> scriptingLists;

	private TimelineManager manager;
	protected override void Init()
	{
		manager = TimelineManager.Instance;
	}

	protected override void EnableCutScene()
	{
		base.EnableCutScene();
	}

	public override void DisableCutScene()
	{
		
	}

	public void Reward_PrintScripts()
	{
		
	}
}
