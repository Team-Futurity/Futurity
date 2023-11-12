using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerManager : MonoBehaviour
{
	public readonly static int MAX_ENEMY_TYPE = 4;
	
	[Header("즉시 스폰 여부")] 
	[SerializeField] private bool isImmediatelySpawn = false;
	
	[Header("컴포넌트")] 
	[SerializeField, ReadOnly()] private List<EnemySpawner> spawnerList = new List<EnemySpawner>();
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private Transform enemyContainer;
	[SerializeField] private DialogData dialogData;

	[Header("Event")] 
	[SerializeField] private bool isUseEvent = true;
	[SerializeField] private int dialogCondition;
	[SerializeField] private UnityEvent spawnEndEvent;
	[SerializeField] private UnityEvent<DialogData> interimEvent;
	[HideInInspector] public bool isEventEnable = false;

	[Header("인디케이터")]
	[SerializeField] private int indicatorCondition = 5;
	[SerializeField] private GameObject indicatorTarget = null;
	private ObjectIndicator objectIndicator;

	[Header("이미 배치된 적이 있다면 사용")] 
	[SerializeField] private List<PlaceEnemy> placeEnemies = null;

	[Header("Debug 패널")] 
	[Tooltip("다음 웨이브 조건"), SerializeField] private int nextWaveCondition = 3;
	[SerializeField] private int curWaveSpawnCount = 0;
	[ReadOnly(false), SerializeField] private int[] totalSpawnCount;
	
	private readonly List<Queue<GameObject>> enemyPool = new List<Queue<GameObject>>();
	
	private void Awake()
	{
		InitSpawnerList();
		
		for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
		{
			enemyPool.Add(new Queue<GameObject>());
		}

		totalSpawnCount = new int[MAX_ENEMY_TYPE];
		InitSpawnerData();
		
		CreateEnemyObject(totalSpawnCount[(int)EnemyType.MeleeDefault], EnemyType.MeleeDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyType.RangedDefault], EnemyType.RangedDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyType.MinimalDefault], EnemyType.MinimalDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyType.EliteDefault], EnemyType.EliteDefault);
		
		objectIndicator = GameObject.FindWithTag("Player").GetComponentInChildren<ObjectIndicator>();
		
		if (placeEnemies.Count == 0)
		{
			return;
		}

		AddAlreadyPlaceEnemy();
	}

	private void Start()
	{
		if (isImmediatelySpawn == false)
		{
			return;
		}
		
		SpawnEnemy();
		Invoke(nameof(EnablePlayerInput), 0.1f);
	}

	public void SpawnEnemy()
	{
		foreach (var spawner in spawnerList)
		{
			spawner.SpawnEnemy();
			curWaveSpawnCount += spawner.GetCurrentSpawnCount();
		}
		
		UpdateSpawnerList();
	}

	public GameObject GetEnemy(EnemyType type)
	{
		if (enemyPool[(int)type].Count <= 0)
		{
			CreateEnemyObject(1, type);
		}
		
		GameObject enemy = enemyPool[(int)type].Dequeue();
		return enemy;
	}

	private void InitSpawnerData()
	{
		foreach (var spawner in spawnerList)
		{
			int[] arr = spawner.GetTotalCreateCount();
			spawner.GetComponent<EnemySpawner>().spawnerDisableEvent.AddListener(SpawnerDisableEvent);

			for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
			{
				totalSpawnCount[i] += arr[i];
			}
		}
	}

	private void CreateEnemyObject(int count, EnemyType type)
	{
		int index = (int)type;

		for (int i = 0; i < count; ++i)
		{
			var enemy = Instantiate(enemyPrefabs[index], enemyContainer);
			enemy.GetComponent<EnemyController>().RegisterEvent(MonsterDisableEvent);
			enemy.SetActive(false);
			
			enemyPool[index].Enqueue(enemy);
		}
	}

	private void MonsterDisableEvent()
	{
		MinusWaveSpawnCount();

		if (curWaveSpawnCount <= 0 && spawnerList.Count <= 0 && isUseEvent == true)
		{
			spawnEndEvent?.Invoke();
			CheckIndicatorDeActivation();
		}
		
		if (curWaveSpawnCount > nextWaveCondition || spawnerList.Count <= 0)
		{
			return;
		}

		foreach (EnemySpawner spawner in spawnerList)
		{
			spawner.SpawnEnemy();
			curWaveSpawnCount += spawner.GetCurrentSpawnCount();
		}
		
		objectIndicator.DeactiveIndicator();
		UpdateSpawnerList();
	}

	private void UpdateSpawnerList()
	{
		List<int> removeIndex = new List<int>();

		for (int i = spawnerList.Count - 1; i >= 0; --i)
		{
			if (spawnerList[i].IsSpawnEnd() == true)
			{
				removeIndex.Add(i);
			}
		}

		foreach (int index in removeIndex)
		{
			spawnerList[index].gameObject.SetActive(false);		
		}
	}

	private void AddAlreadyPlaceEnemy()
	{
		foreach (PlaceEnemy enemyInfo in placeEnemies)
		{
			enemyInfo.enemyObj.GetComponent<EnemyController>().RegisterEvent(MonsterDisableEvent);
			enemyPool[(int)enemyInfo.enemyType].Enqueue(enemyInfo.enemyObj);
			curWaveSpawnCount++;
		}
	}
	
	private void SpawnerDisableEvent(EnemySpawner spawner)
	{
		spawnerList.Remove(spawner);
	}

	private void MinusWaveSpawnCount()
	{
		curWaveSpawnCount--;
		CheckIndicatorActivation();
		
		if (curWaveSpawnCount >= dialogCondition || isEventEnable == true)
		{
			return;
		}
		
		isEventEnable = true;
		interimEvent?.Invoke(dialogData);
	}

	private void CheckIndicatorActivation()
	{
		if (curWaveSpawnCount > indicatorCondition || objectIndicator.IsActive == true)
		{
			return;
		}
		
		objectIndicator.ActivateIndicator();
	}

	private void CheckIndicatorDeActivation()
	{
		if (indicatorTarget == null)
		{
			objectIndicator.DeactiveIndicator();
		}
		else
		{
			objectIndicator.DeactiveIndicator();
			objectIndicator.ActivateIndicator(indicatorTarget);
		}
	}
	
	private void EnablePlayerInput()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}

	private void InitSpawnerList()
	{
		spawnerList.Clear();
		
		for (int i = 0; i < transform.childCount; ++i)
		{
			if (transform.GetChild(i).TryGetComponent(out EnemySpawner enemySpawner) == true)
			{
				spawnerList.Add(enemySpawner);
			}
		}
	}
}

[Serializable]
public struct PlaceEnemy
{
	public EnemyType enemyType;
	public GameObject enemyObj;
}
