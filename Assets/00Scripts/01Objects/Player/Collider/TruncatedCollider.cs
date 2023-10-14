using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class TruncatedCollider<T> : ColliderBase where T : Collider
{
	public T ColliderReference { get; private set; }

	protected virtual void Awake()
	{
		ColliderReference = gameObject.AddComponent<T>();
		ColliderReference.isTrigger = IsTrigger;
		ColliderReference.enabled = false;
	}

	public override abstract void SetColliderActivation(bool isActive);

	public override abstract void SetCollider(float angle, float radius);

	// ���� �ݶ��̴� ���� ���Դ��� �����ϴ� �޼ҵ�
	public override abstract bool IsInCollider(GameObject target);

	// �߶� �ݶ��̴� ���� �ִ��� �����ϴ� �޼ҵ�
	public override bool IsInCuttedCollider(GameObject target, bool ignoresCut = false)
	{
		if (ignoresCut) { return true; }

		Vector3 targetVec = target.transform.position - transform.position;

		if (targetVec.magnitude > Length) { return false; }

		float dot = Vector3.Dot(targetVec.normalized, transform.forward);
		float theta = Mathf.Acos(dot);

		var vecs = GetVectorToCut();

		Vector3 leftPos = transform.position + vecs[1];
		Vector3 rightPos = transform.position + vecs[0];

		Ray leftRay = new Ray(transform.position, (leftPos - transform.position).normalized);
		Ray rightRay = new Ray(transform.position, (rightPos - transform.position).normalized);

		var leftRayCaster = Physics.Raycast(leftRay, Length);
		var rightRayCaster = Physics.Raycast(rightRay, Length);

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
	// 0��° �ε��� : Right
	// 1��° �ε��� : Left
	public virtual Vector3[] GetVectorToCut(Vector3? originPos = null)
	{
		Vector3 origin = originPos ?? Vector3.zero;
		Vector3[] vecs = new Vector3[2];
		float posX, posZ;
		float posY = 0;
		float halfAngle = Angle * 0.5f;
		float theta = 90 - transform.eulerAngles.y;

		// right
		posX = Mathf.Cos((theta - halfAngle) * Mathf.Deg2Rad) * Length;
		posZ = Mathf.Sin((theta - halfAngle) * Mathf.Deg2Rad) * Length;
		vecs[0] = new Vector3(posX, posY, posZ) + origin;

		// left
		posX = Mathf.Cos((theta + halfAngle) * Mathf.Deg2Rad) * Length;
		posZ = Mathf.Sin((theta + halfAngle) * Mathf.Deg2Rad) * Length;
		vecs[1] = new Vector3(posX, posY, posZ) + origin;

		return vecs;
	}
}
