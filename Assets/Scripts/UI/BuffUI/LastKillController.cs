using System.Collections.Generic;
using UnityEngine;

public class LastKillController : MonoBehaviour
{
	[SerializeField] private List<GameObject> deActivePortals;

	void Start()
	{
		StageEndPotalManager.Instance.SetLastKillController(this);
	}
	
	private void LastKill()
	{
		TimelineManager.Instance.EnableCutScene(TimelineManager.ECutScene.LastKillCutScene);
	}
	
	public void PortalActive()
	{
		if (deActivePortals.Count != 0)
		{
			deActivePortals[0].GetComponent<StageEndPotalController>().isActiveStageEndPortal = true;
		}
	}
}
