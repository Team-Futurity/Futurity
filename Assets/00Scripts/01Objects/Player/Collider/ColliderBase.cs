using UnityEngine;

[System.Serializable]
public abstract class ColliderBase : MonoBehaviour
{
	[field: SerializeField] public bool IsTrigger { get; protected set; }
	[SerializeField] protected Color colliderColor = Color.red;


	// ���� ���� ǥ��
#if UNITY_EDITOR
	protected abstract void OnDrawGizmos();
#endif
}