using UnityEngine;

public class InitializeSingleton : MonoBehaviour
{
	private void Awake()
	{
		var _ = Singleton.Instance;
	}
}