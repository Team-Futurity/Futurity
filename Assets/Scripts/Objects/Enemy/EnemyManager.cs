using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
	public List<EnemyController> activeEnemys;
	public List<Cluster> clusters;
	public List<EnemyController> clusterElse;


	public void ActiveManagement(EnemyController unit)
	{
		activeEnemys.Add(unit);
	}

	public void DeActiveManagement(EnemyController unit)
	{
		activeEnemys.Remove(unit);
	}

	public void EnemyClustering(EnemyController unit)
	{
		clusterElse.Add(unit);

		if(clusterElse.Count > 3)
		{
			clusters.Add(new Cluster());
			for (int i = 0; i < 3; i++)
			{
				clusterElse[i].isClustering = true;
				clusterElse[i].navMesh.avoidancePriority = i * 10;
				clusterElse[i].clusterNum = clusters.Count - 1;
				clusterElse[i].individualNum = i;

				if (i > 0)
				{
					clusterElse[i].clusterTarget = clusterElse[i - 1];
					clusterElse[i].ChangeState(EnemyController.EnemyState.ClusterChase);
				}

				clusters[clusters.Count - 1].enemys.Add(clusterElse[i]);
			}
			clusterElse.RemoveRange(0, 3);
		}
	}

	public void EnemyDeclutter(int clusterNum)
	{
		for (int i = 0; i < clusters[clusterNum].enemys.Count; i++)
			clusters[clusterNum].enemys[i].isClustering = false;

		clusters.RemoveAt(clusterNum);
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
}