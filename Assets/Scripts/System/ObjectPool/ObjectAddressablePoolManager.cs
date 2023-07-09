 using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class ObjectAddressablePoolManager<PoolingClass> : OBJAddressablePoolParent where PoolingClass : Component
{
    private List<PoolingClass> activedPoolingObjects = new List<PoolingClass>();
	private Stack<PoolingClass> nonActivedObjects =  new Stack<PoolingClass>();

	public List<PoolingClass> ActivePoolingObjects => activedPoolingObjects;

    [SerializeField] private int activeObjCount;

    public ObjectAddressablePoolManager(AssetReference prefab, GameObject parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        activeObjCount = 0;
    }

	public void SetManager(AssetReference prefab, GameObject parent = null)
	{
		this.prefab = prefab;
		this.parent = parent;
		activeObjCount = 0;
	}

    public PoolingClass ActiveObject(ref AsyncOperationHandle<GameObject> effectObject, Vector3? startPos = null, Quaternion? startRot = null)
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
				returnValue = obj;
            }
		}
        else
        {
			// �� ������Ʈ ����
			effectObject = prefab.InstantiateAsync(parent == null ? null : parent.transform);// GameObject.Instantiate(prefab, parent == null ? null : parent.transform);

			/*newObj.Completed +=
				(AsyncOperationHandle<GameObject> handle) =>
				{
					

					// ������ Ȱ��ȭ�� ������Ʈ �÷���
					if (returnValue != null)
					{
						activeObjCount++;
						returnValue.gameObject.transform.position = (Vector3)startPos;
						returnValue.gameObject.transform.rotation = (Quaternion)startRot;
						activedPoolingObjects.Add(returnValue);
					}
				};*/
			effectObject.WaitForCompletion();

			PoolingObject poolObj;
			GameObject obj = effectObject.Result;
			obj.TryGetComponent(out poolObj);

			// PoolingObject�� ���
			if (poolObj != null)
			{
				// ���� üũ�� ������ �Ƹ� �߻� ���� ��?
				dynamic poolManager = this;

				poolObj.Initialize(poolManager);
				poolObj.ActiveObj();
			}
			returnValue = obj.GetComponent<PoolingClass>();
		}
        
        // ������ Ȱ��ȭ�� ������Ʈ �÷���
        if(returnValue != null)
        {
            activeObjCount++;
			returnValue.gameObject.transform.position = (Vector3)startPos;
			returnValue.gameObject.transform.rotation = (Quaternion)startRot;
			activedPoolingObjects.Add(returnValue);
		}

        return returnValue;
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