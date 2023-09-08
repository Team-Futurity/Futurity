using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
	public List<EnemyController> activeEnemys;
	public List<Cluster> clusters;
	public List<EnemyController> clusterElse;


	// 리스트를 퍼블릭으로 관리하지 않는다.
	// 리스트 널 에이블 처리가 안되었다.
	
	// 단순하게 에너미 컨트롤러만 수집하기에 어떤 타입의 몬스터가 있는지 파악이 힘들다.
	// 보스 몬스터 관리가 없다.
	
	// 클러스팅 도대체 뭐임
	// 
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

	// ?? 
	public void EnemyDeclutter(int clusterNum)
	{
		for (int i = 0; i < clusters[clusterNum].enemys.Count; i++)
			clusters[clusterNum].enemys[i].isClustering = false;

		clusters.RemoveAt(clusterNum);
	}
}

// 클래스는 새로운 파일을 통해서 분리한다.

// 이외에 관리를 위해서 필요한 메서드가 무엇이 있을지 고민.........
[Serializable]
public class Cluster
{
	public List<EnemyController> enemys;

	public Cluster()
	{
		enemys = new List<EnemyController>();
	}
}