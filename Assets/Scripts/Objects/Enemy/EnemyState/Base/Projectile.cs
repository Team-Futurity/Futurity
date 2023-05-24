using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private EnemyController ec;

	private void Update()
    {
		transform.position += transform.forward * ec.projectileSpeed * Time.deltaTime;
	}
}
