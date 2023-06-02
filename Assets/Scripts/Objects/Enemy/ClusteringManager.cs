using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusteringManager : Singleton<ClusteringManager>
{
	public List<Cluster> clusters;
	public List<EnemyController> elseEnemy;


	public void AddEnemyInManager(EnemyController unit)
	{
		elseEnemy.Add(unit);

		if(elseEnemy.Count > 3)
		{
			clusters.Add(new Cluster());
			for (int i = 0; i < 3; i++)
			{
				clusters[clusters.Count - 1].enemys.Add(elseEnemy[i]);
				elseEnemy[i].isClustering = true;
				elseEnemy[i].navMesh.avoidancePriority = i * 10;
				elseEnemy[i].clusterNum = clusters.Count - 1;
				elseEnemy[i].individualNum = i;
				if(i > 0)
					elseEnemy[i].ChangeState(EnemyController.EnemyState.MDefaultClusterChase);
			}
			elseEnemy.RemoveRange(0, 3);
		}
	}
}

[Serializable]
public class Cluster
{
	public List<EnemyController> enemys;

	public Cluster()
	{
		enemys = new List<EnemyController>();
	}

	public void AddToClusters(EnemyController unit)
	{
		enemys.Add(unit);
	}
}