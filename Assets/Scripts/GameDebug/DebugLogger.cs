
using System;
using System.Collections.Generic;

public static class DebugLogger
{
	public static void PrintDebugErros(List<string> msgs, Type componentType = null, DebugTypeEnum debugType = DebugTypeEnum.Log)
	{
		string name = componentType == null ? "System" : componentType.Name;

		foreach (string msg in msgs)
		{
			switch(debugType)
			{
				case DebugTypeEnum.Log:
					FDebug.Log($"[{name}] {msg}");
					break;
				case DebugTypeEnum.Warning:
					FDebug.LogWarning($"[{name}] {msg}");
					break;
				case DebugTypeEnum.Error:
					FDebug.LogError($"[{name}] {msg}");
					break;

			}
		}
	}
}
