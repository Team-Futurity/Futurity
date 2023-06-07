using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePartController : MonoBehaviour
{
	public List<ActivePartData> activePartDatas;
	private Dictionary<ActivePartType, ActivePartProccessor> activeParts;

	private void Awake()
	{
		activeParts = new Dictionary<ActivePartType, ActivePartProccessor>();
		for(int count = 0; count < activePartDatas.Count; count++)
		{
			if (activeParts.ContainsKey(activePartDatas[count].type)) { continue; }
			activeParts.Add(activePartDatas[count].type, activePartDatas[count].proccessor);
		}
	}

	public void RunActivePart(PlayerController pc, Player p, ActivePartType type)
	{
		activeParts[type].RunActivePart(pc, activeParts[type]);
	}
}
