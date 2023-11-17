using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager<PoolingClass> : OBJPoolParent where PoolingClass : Component
{
    private List<PoolingClass> activedPoolingObjects = new List<PoolingClass>();
	private Stack<PoolingClass> nonActivedObjects =  new Stack<PoolingClass>();

	public List<PoolingClass> ActivePoolingObjects => activedPoolingObjects;

    [SerializeField] private int activeObjCount;

    public ObjectPoolManager(GameObject prefab, GameObject parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        activeObjCount = 0;
    }

	public void SetManager(GameObject prefab, GameObject parent = null)
	{
		this.prefab = prefab;
		this.parent = parent;
		activeObjCount = 0;
	}

	public void ChangeParent(GameObject parent)
	{
		this.parent = parent;
	}

	public PoolingClass ActiveObject(Vector3? startPos = null, Quaternion? startRot = null, bool isWorld = true)
    {
		PoolingClass returnValue = null;
		startPos = startPos ?? Vector3.zero;
		startRot = startRot ?? Quaternion.identity;

		// ��Ȱ��ȭ�� ������Ʈ�� �����ִٸ�
		if (nonActivedObjects.Count > 0)
        {
			PoolingClass obj = nonActivedObjects.Pop();

            // ������Ʈ�� �����ϰ� ��Ȱ��ȭ ���¶��
            if (obj != null)
            {
                // PoolingObject�� ���
                if (obj.GetType() == typeof(PoolingObject))
                {
                    // PoolingObject�� ��ȯ
                    PoolingObject poolObj;
                    obj.TryGetComponent(out poolObj);

                    // ��ȯ ������
                    if (poolObj != null)
                    {
                        poolObj.ActiveObj(); // Active�� �ߵ��Ǵ� �۾�
                    }
                }
                // Ȱ��ȭ
                obj.gameObject.SetActive(true);

				if(parent != null)
				{
					obj.transform.parent = parent.transform;
				}

				returnValue = obj;
            }
        }
        else
        {
            // �� ������Ʈ ����
            GameObject newObj = GameObject.Instantiate(prefab, parent == null ? null : parent.transform);

            PoolingObject poolObj;
            newObj.TryGetComponent(out poolObj);

            // PoolingObject�� ���
            if (poolObj != null)
            {
                // ���� üũ�� ������ �Ƹ� �߻� ���� ��?
                dynamic poolManager = this;

                poolObj.Initialize(poolManager);
                poolObj.ActiveObj();
            }
            returnValue = newObj.GetComponent<PoolingClass>();
        }
        
        // ������ Ȱ��ȭ�� ������Ʈ �÷���
        if(returnValue != null)
        {
            activeObjCount++;

			if (isWorld)
			{
				SetObjectTransform(returnValue.gameObject, startPos, startRot);
			}
			else
			{
				SetObjectLocalTransform(returnValue.gameObject, startPos, startRot);
			}

			activedPoolingObjects.Add(returnValue);
		}
        return returnValue;
    }

	public void SetObjectTransform(GameObject target, Vector3? pos = null, Quaternion? rot = null)
	{
		Vector3 startPosistion = pos ?? Vector3.zero;
		Quaternion startRotation = rot ?? Quaternion.identity;

		target.transform.position = startPosistion;
		target.transform.rotation = startRotation;
	}

	public void SetObjectLocalTransform(GameObject target, Vector3? pos = null, Quaternion? rot = null)
	{
		Vector3 startPosistion = pos ?? Vector3.zero;
		Quaternion startRotation = rot ?? Quaternion.identity;

		target.transform.localPosition = startPosistion;
		target.transform.localRotation = startRotation;
	}

	public void DeactiveObject(PoolingClass target)
    {
        activeObjCount--;

        PoolingObject poolObj;
        target.TryGetComponent(out poolObj);

		if(!activedPoolingObjects.Remove(target))
		{
			FDebug.LogWarning("[ObjectPoolManager] �ش� ������Ʈ�� ���� ������Ʈ Ǯ������ �����ϰ� �ִ� ������Ʈ�� �ƴϰų�, Ȱ��ȭ�Ǿ� ���� �ʽ��ϴ�.");
			return;
		}

		// PoolingObject�� ���
		if (poolObj != null)
        {
            poolObj.DeactiveObj();
        }
        target.gameObject.SetActive(false);
		nonActivedObjects.Push(target);
    }
}