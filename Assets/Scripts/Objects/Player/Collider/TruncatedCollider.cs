using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class TruncatedCollider<T> : MonoBehaviour where T : Collider
{
	[SerializeField] private float angle;

	public bool IsTrigger { get; protected set; }
	public float Angle
	{
		get { return angle; }
		protected set
		{
			if (value == 360) { angle = 360; }
			else { angle = value % 360;	}
		}
	}
	[field : SerializeField] public float Radius { get; protected set; }
	public T truncatedCollider { get; private set; }
	[SerializeField] protected Color colliderColor = Color.red;

	protected virtual void Start()
	{
		truncatedCollider = gameObject.AddComponent<T>();
		truncatedCollider.isTrigger = IsTrigger;
		truncatedCollider.enabled = false;
	}

	// �ݶ��̴� �⺻ ����
	/*public void SetCollider(float angle, float radius)
	{
		this.angle = angle;
		this.radius = radius;

		radiusCollider.radius = radius;
	}
	*/

	// ���� �ݶ��̴� ���� ���Դ��� �����ϴ� �޼ҵ�
	public abstract bool IsInCollider(GameObject target);

	// �߶� �ݶ��̴� ���� �ִ��� �����ϴ� �޼ҵ�
	public virtual bool IsInCuttedCollider(GameObject target)
	{
		Vector3 targetVec = target.transform.position - transform.position;

		if (targetVec.magnitude > Radius) { return false; }

		float dot = Vector3.Dot(targetVec.normalized, transform.forward);
		float theta = Mathf.Acos(dot);

		var vecs = GetVectorToCut();

		Vector3 leftPos = transform.position + vecs[1];
		Vector3 rightPos = transform.position + vecs[0];

		Ray leftRay = new Ray(transform.position, (leftPos - transform.position).normalized);
		Ray rightRay = new Ray(transform.position, (rightPos - transform.position).normalized);

		var leftRayCaster = Physics.Raycast(leftRay, Radius);
		var rightRayCaster = Physics.Raycast(rightRay, Radius);

		if (leftRayCaster || rightRayCaster) { return true; }

		return theta * 2 <= (angle == 0 && Angle > 0 ? 360 : angle) * Mathf.Deg2Rad;
	}

	// �ش� ������Ʈ���� �߶� �ݶ��̴� ���� �ִ��� �����ϴ� �޼ҵ�
	public virtual List<GameObject> GetObjectsInCollider(List<GameObject> targets)
	{
		List<GameObject> objects = targets.ToList();
		for (int targetCount = 0; targetCount < objects.Count; targetCount++)
		{
			if (!IsInCuttedCollider(objects[targetCount]))
			{
				objects.RemoveAt(targetCount);
			}
		}

		return objects;
	}

	// �ڱ� �ڽ��� �������� ��輱 ���͸� �˾Ƴ��� �޼ҵ�
	// ��輱�� �¿� �� �� �����̸� ��輱�� ������ ����� �ڸ�
	public virtual Vector3[] GetVectorToCut()
	{
		Vector3[] vecs = new Vector3[2];
		float halfAngle = Angle * 0.5f;
		float posX, posY, posZ;
		float theta = 90 - transform.eulerAngles.y;

		posY = 0;

		// right
		posX = Mathf.Cos((theta - halfAngle) * Mathf.Deg2Rad) * Radius;
		posZ = Mathf.Sin((theta - halfAngle) * Mathf.Deg2Rad) * Radius;
		vecs[0] = new Vector3(posX, posY, posZ);

		// left
		posX = Mathf.Cos((theta + halfAngle) * Mathf.Deg2Rad) * Radius;
		posZ = Mathf.Sin((theta + halfAngle) * Mathf.Deg2Rad) * Radius;
		vecs[1] = new Vector3(posX, posY, posZ);

		return vecs;
	}

	// ���� ���� ǥ��
#if UNITY_EDITOR
	protected abstract void OnDrawGizmos();
#endif
}
