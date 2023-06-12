using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;
	private static object _lock = new object();
	private static bool _applicationQuit = false;

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

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
						string componentName = typeof(T).ToString();

						GameObject findObject = GameObject.Find(componentName);

						if (findObject == null)
						{
							findObject = new GameObject(componentName);
						}

						_instance = findObject.AddComponent<T>();

						DontDestroyOnLoad(_instance);
					}
				}

				return _instance;
			}
		}
	}

	protected void OnApplicationQuit()
	{
		_applicationQuit = true;
	}

	// OnDestroy() 메서드에서 _applicationQuit 부분을 제거합니다.
	public void OnDestroy() { }

	protected virtual void OnEnable()
	{
		Application.quitting += OnApplicationQuitting;
	}

	protected virtual void OnDisable()
	{
		Application.quitting -= OnApplicationQuitting;
	}

	private void OnApplicationQuitting()
	{
		_applicationQuit = true;
	}
}
