using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float projectileSpeed = 10.0f;
	private void Update()
    {
		transform.position += transform.forward * projectileSpeed * Time.deltaTime;
	}
}
