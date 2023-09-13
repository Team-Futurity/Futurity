using UnityEngine;

public class DrawPosition : MonoBehaviour
{
	public Color color;
	public float radius = 0.5f;
	
	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position + (Vector3.up * 0.5f), radius);
	}
}
