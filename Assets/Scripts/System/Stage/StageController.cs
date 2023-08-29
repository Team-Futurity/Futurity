using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
	// Monster Action ����
	// StageController�� UID
	[field: SerializeField] public int StageID { get; private set; } 
	[field: SerializeField] public GameObject Spawner { get; private set; }
	
    // Stage�� ���۰� ���Ḧ �����Ѵ�. [T] : Start, [F] : END
    [HideInInspector] public UnityEvent<bool> onGameState;
    
    // Stage�� �����ϴ� Monster�� �����Ѵ�.
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
	    BeforeStartStage();
	    onGameState?.Invoke(true);
    }

    public void EndStage()
    {
	    BefroeEndStage();
	    onGameState?.Invoke(false);
    }
    
    private void BeforeStartStage()
    {
	    
    }

    private void BefroeEndStage()
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
		    // ������ ���͸� ������ ���, ������ ����.
		    
	    }
	    else
	    {
		    // ������ ���͸� ���� ��찡 �ƴ� ���. �ܼ� ����
		    
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
