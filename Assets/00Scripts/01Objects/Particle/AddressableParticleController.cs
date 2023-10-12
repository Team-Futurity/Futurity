using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressableParticleController : MonoBehaviour
{
	public ObjectAddressablePoolManager<Transform> poolManager;
	private ParticleSystem ps;

	private void OnEnable()
	{
		ps.Play(true);

	}
	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Initialize(ObjectAddressablePoolManager<Transform> poolManager)
	{
		this.poolManager = poolManager;
	}

	private void Update()
	{
		if (poolManager != null)
		{
			if (!ps.IsAlive(true))
			{
				ps.Stop(true);
				poolManager.DeactiveObject(transform);
			}
		}
	}
}
