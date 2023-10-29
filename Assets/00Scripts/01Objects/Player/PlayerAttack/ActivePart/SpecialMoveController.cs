using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMoveController : MonoBehaviour
{
	public List<ActivePartData> activePartDatas;
	private Dictionary<SpecialMoveType, SpecialMoveProcessor> specialMoves;

	private void Awake()
	{
		specialMoves = new Dictionary<SpecialMoveType, SpecialMoveProcessor>();
		
		// Active Part Data를 저장한다.
		for(int count = 0; count < activePartDatas.Count; count++)
		{
			if (specialMoves.ContainsKey(activePartDatas[count].type)) { continue; }
			specialMoves.Add(activePartDatas[count].type, activePartDatas[count].proccessor);
		}
	}

	public void RunActivePart(PlayerController pc, Player p, SpecialMoveType type)
	{
		// SpeicalMoves는 Data를 보관하고 있는 Dictionary Type
		// 
		specialMoves[type].RunSpecialMove(pc, specialMoves[type]);
	}
}
