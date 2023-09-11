using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterManager : Singleton<ClusterManager>
{
	private List<Cluster> clusters;
	public List<Cluster> ClusterList => clusters;
	private List<EnemyController> clusterElse;
	
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

				clusters[^1].enemyList.Add(clusterElse[i]);
			}
			clusterElse.RemoveRange(0, 3);
		}
	}
	
	public void EnemyDeCluster(int clusterNum)
	{
		for (int i = 0; i < clusters[clusterNum].enemyList.Count; ++i)
		{
			clusters[clusterNum].enemyList[i].isClustering = false;
		}

		clusters.RemoveAt(clusterNum);
	}
}
