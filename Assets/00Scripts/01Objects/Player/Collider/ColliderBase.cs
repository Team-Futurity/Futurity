using UnityEngine;

[System.Serializable]
public abstract class ColliderBase : MonoBehaviour
{
	[field: SerializeField] public bool IsTrigger { get; protected set; }
	[SerializeField] protected Color colliderColor = Color.red;

	// data
	[SerializeField] protected float angle;
	public abstract float Angle {  get; protected set; }

	public abstract float Length { get; protected set; }

	// methods
	public abstract void SetColliderActivation(bool isActive);

	public abstract void SetCollider(float angle, float radius);

	public abstract bool IsInCollider(GameObject target);

	public abstract bool IsInCuttedCollider(GameObject target, bool ignoresCut);

	// 공격 범위 표시
#if UNITY_EDITOR
	protected abstract void OnDrawGizmos();
#endif
}