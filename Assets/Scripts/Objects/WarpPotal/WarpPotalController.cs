using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpPotalController : MonoBehaviour
{
	public bool isActivePotal = true;
	[SerializeField] 
	private Transform targetPosition;
	[SerializeField] 
	private bool isSceneChanger = false;
	[SerializeField] 
	private SceneKeyData chageSceneKeyData;
	[SerializeField] 
	private UnityEvent warpStartEvent;
	[SerializeField] 
	private UnityEvent warpEndEvent;

	public float delay = 0f;


	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("Player") && isActivePotal)
		{
			warpStartEvent?.Invoke();
			if (!isSceneChanger)
			{
				AreaWarpManager.Instance.WarpStart(collision.gameObject, targetPosition, delay, warpEndEvent);
			} else
			{
				if (chageSceneKeyData)
				{
					SceneChangeManager.Instance.SceneLoad(chageSceneKeyData);
					warpEndEvent?.Invoke();
				}
				else
				{
					FDebug.LogWarning($"{gameObject.name}에 chageSceneKeyData가 없습니다.");
				}
			}
		}
	}
}
