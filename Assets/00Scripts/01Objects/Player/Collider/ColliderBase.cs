using UnityEngine;

[System.Serializable]
public abstract class ColliderBase : MonoBehaviour
{
	[field: SerializeField] public bool IsTrigger { get; protected set; }
	[SerializeField] protected Color colliderColor = Color.red;


	// 공격 범위 표시
#if UNITY_EDITOR
	protected abstract void OnDrawGizmos();
#endif
}