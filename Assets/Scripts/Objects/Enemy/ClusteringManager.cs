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
				clusters[clusters.Count - 1].enemys.Add(new EnemyController());
				clusters[clusters.Count - 1].enemys[i] = elseEnemy[i];
				elseEnemy.RemoveAt(i);
			}
		}
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