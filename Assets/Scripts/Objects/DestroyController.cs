using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    //#����#  �ش� ��ũ��Ʈ�� ������Ʈ�� �ġڱ��ٸ� ����ϴ� ��ũ��Ʈ�Դϴ�. 


    [SerializeField]
    [Tooltip("0. No Destory \n1. DelayDestroy")]
    int setDestroy = 1;

    [SerializeField]
    [Tooltip("������Ʈ�� �ı� �� �ð��� �����Ѵ�.")]
    float destroyDelayTime = 1;



    private void Start()
    {
        switch(setDestroy)
        {
            case 1:
                StartCoroutine(DelayDestroy(gameObject, destroyDelayTime));
                break;

			default:
				break;
		}
    }

	public void DestroyObject(GameObject destroyObject)
	{
		StartCoroutine(DelayDestroy(destroyObject, destroyDelayTime));
	}
	public void ThisDestroy()
	{
		StartCoroutine(DelayDestroy(this.gameObject, destroyDelayTime));
	}

	IEnumerator DelayDestroy (GameObject destroyObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(this.gameObject);
    }
}
