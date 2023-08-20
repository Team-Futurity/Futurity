using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// StageController의 UID
	[field: SerializeField] public int StageID { get; private set; } 
	
    // Stage의 시작과 종료를 관리한다. [T] : Start, [F] : END
    [HideInInspector] public UnityEvent<bool> onGameState;
    
    // Stage에 존재하는 Monster를 관리한다.
    private Dictionary<int, Enemy> activeEnemyDic;
    private Dictionary<int, Enemy> allEnemyDic;

    private bool onlyLastMonsterRemain = false;

    private void Awake()
    {
	    activeEnemyDic = new Dictionary<int, Enemy>();
	    allEnemyDic = new Dictionary<int, Enemy>();
    }
    
    public void StartStage()
    {
    }

    public void EndStage()
    {
	    
    }

    public void AddEnemy(Enemy enemy)
    {
	    var uID = enemy.GetInstanceID();
	    
	    allEnemyDic.Add(uID, enemy);
	    activeEnemyDic.Add(allEnemyDic.Last().Key, allEnemyDic.Last().Value);
    }

    public void RemoveEnemy(Enemy enemy)
    {
	    var uID = enemy.GetInstanceID();

	    if (onlyLastMonsterRemain)
	    {
		    // 마지막 몬스터만 남았을 경우, 마지막 연출.
		    
	    }
	    else
	    {
		    // 마지막 몬스터만 남은 경우가 아닐 경우. 단순 제거
		    
		    activeEnemyDic?.Remove(uID);
	    }
	    
	    
	    UpdateLastEnemy();
    }

    private void UpdateLastEnemy()
    {
	    if (allEnemyDic.Count == 1)
	    {
		    
	    }
    }
}
