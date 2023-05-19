using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    //#설명#  해당 스크립트는 오브젝트의 파★괴☆를 담당하는 스크립트입니다. 


    [SerializeField]
    [Tooltip("0. No Destory \n1. DelayDestroy")]
    int setDestroy = 1;

    [SerializeField]
    [Tooltip("오브젝트가 파괴 될 시간을 지정한다.")]
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
