using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusteringManager : Singleton<ClusteringManager>
{
	public List<Cluster> clusters;
	public List<EnemyController> elseEnemy;


	private void Start()
	{
		/*for (int i = 0; i < elseEnemy.Count; i++)
			AddToCluster(elseEnemy[i]);*/
	}
}

[Serializable]
public class Cluster
{
	public List<EnemyController> enemys;

	public void AddToClusters(EnemyController unit)
	{
		enemys.Add(unit);
	}
}