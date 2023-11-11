using UnityEngine;
using UnityEngine.Events;

public class ColliderEvent : MonoBehaviour
{
	[SerializeField] private UnityEvent onEnterEvent;
	[SerializeField] private UnityEvent onStayEvent;
	[SerializeField] private UnityEvent onExitEvent;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		onEnterEvent?.Invoke();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
	
		onStayEvent?.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
	
		onExitEvent?.Invoke();
	}
}
