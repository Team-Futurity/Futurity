using UnityEngine;

public class FDebug
{
	public static bool isDebugBuild
	{
		get { return UnityEngine.Debug.isDebugBuild; }
	}


	public static void Log(object message)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log(message);
#endif
	}

	public static void Break()
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Break();
#endif
	}


	public static void Log(object message, UnityEngine.Object context)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log(message, context);
#endif
	}


	public static void LogFormat(string format, params object[] args)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.LogFormat(format, args);
#endif
	}


	public static void LogError(object message)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.LogError(message);
#endif
	}


	public static void LogError(object message, UnityEngine.Object context)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.LogError(message, context);
#endif
	}


	public static void LogWarning(object message)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.LogWarning(message.ToString());
#endif
	}


	public static void LogWarning(object message, UnityEngine.Object context)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.LogWarning(message.ToString(), context);
#endif
	}


	public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f,
		bool depthTest = true)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
#endif
	}


	public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f,
		bool depthTest = true)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
#endif
	}


	public static void Assert(bool condition)
	{
#if UNITY_EDITOR
		if (!condition) throw new System.Exception();
#endif
	}
}