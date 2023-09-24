using UnityEngine;
using System;

public class FDebug
{
	private const string NULLTYPEHEADER = "UNKNOWN";

	public static bool isDebugBuild
	{
		get { return Debug.isDebugBuild; }
	}

	public static void Break()
	{
#if UNITY_EDITOR
		Debug.Break();
#endif
	}

	#region Logging
	public static void Log(object message, Type executionType = null)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.Log($"[{name}] {message}");
#endif
	}

	public static void Log(object message, UnityEngine.Object context, Type executionType = null)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.Log($"[{name}] {message}", context);
#endif
	}


	public static void LogFormat(string format, Type executionType = null, params object[] args)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.LogFormat($"[{name}] {format}", args);
#endif
	}


	public static void LogError(object message, Type executionType = null)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.LogError($"[{name}] {message}");
#endif
	}


	public static void LogError(object message, UnityEngine.Object context, Type executionType = null)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.LogError($"[{name}] {message}", context);
#endif
	}


	public static void LogWarning(object message, Type executionType = null)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.LogWarning($"[{name}] {message}");
#endif
	}


	public static void LogWarning(object message, UnityEngine.Object context, Type executionType = null)
	{
#if UNITY_EDITOR
		string name = executionType == null ? NULLTYPEHEADER : executionType.Name;
		Debug.LogWarning($"[{name}] {message}", context);
#endif
	}
	#endregion

	public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f,
		bool depthTest = true)
	{
#if UNITY_EDITOR
		Debug.DrawLine(start, end, color, duration, depthTest);
#endif
	}


	public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f,
		bool depthTest = true)
	{
#if UNITY_EDITOR
		Debug.DrawRay(start, dir, color, duration, depthTest);
#endif
	}


	public static void Assert(bool condition)
	{
#if UNITY_EDITOR
		if (!condition) throw new System.Exception();
#endif
	}
}