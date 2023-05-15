using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
	public ObjectPoolManager<Transform> poolManager;
	private ParticleSystem ps;

	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Initialize(ObjectPoolManager<Transform> poolManager)
	{
		this.poolManager = poolManager;
	}

	private void Update()
	{
		if (poolManager != null) 
		{
			if(!ps.IsAlive(true))
			{
				poolManager.DeactiveObject(transform);
			}
		}
	}
}
