using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static object _lock = new object();
	private static bool _applicationQuit = false;

	public static T Instance
	{
		get
		{
			if (_applicationQuit)
			{
				return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();

					if (_instance == null)
					{
						GameObject singleton = new GameObject(typeof(T).ToString());
						_instance = singleton.AddComponent<T>();
						DontDestroyOnLoad(singleton);
					}
				}
				return _instance;
			}
		}
	}

	protected virtual void OnApplicationQuit()
	{
		_applicationQuit = true;
	}

	protected virtual void OnDestroy()
	{
		_applicationQuit = true;
	}
}
