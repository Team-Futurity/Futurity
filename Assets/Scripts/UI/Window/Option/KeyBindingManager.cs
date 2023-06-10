using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class KeyBindingManager : MonoBehaviour
{
	private Dictionary<InputActionReference, string> pendingBindings = new Dictionary<InputActionReference, string>();

	[SerializeField] 
	private Dictionary<InputActionReference, List<string[]>> pendingMultiBindings = new Dictionary<InputActionReference, List<string[]>>();


	public void RegisterPendingBinding(InputActionReference actionReference, string key)
	{
		//#설명# 이 함수는 키 바인딩을 아직 적용되지 않은 바인딩 리스트에 등록합니다.
		pendingBindings[actionReference] = key;
	}

	public void RegisterMovePendingBinding(InputActionReference actionReference, string key, string actionName)
	{
		//#설명# 이 함수는 움직임을 담당하는 키의 바인딩을 아직 적용되지 않은 바인딩 리스트에 등록합니다.
		if (!pendingMultiBindings.ContainsKey(actionReference))
		{
			pendingMultiBindings[actionReference] = new List<string[]>();
		}

		pendingMultiBindings[actionReference].Add(new string[] { key, actionName });
	}

	public void ApplyBindings()
	{
		//#설명# 이 함수는 아직 적용되지 않은 모든 키 바인딩을 적용합니다.
		foreach (var bindingObject in pendingBindings)
		{
			var action = bindingObject.Key.action;

			action.ApplyBindingOverride(bindingObject.Value);
		}

		foreach (var bindingObject in pendingMultiBindings)
		{
			var action = bindingObject.Key.action;

			for (int i = 0; i < bindingObject.Value.Count; i++)
			{
				var bindingIndex = action.bindings.IndexOf(binding => binding.name == bindingObject.Value[i][1]);

				if (bindingIndex != -1)
				{

					FDebug.Log($"bindingIndex.path : {bindingObject.Value[i][0]} \nbindingIndex.name : {bindingObject.Value[i][1]}");
					string overridePathText = bindingObject.Value[i][0];
					FDebug.Log($"overridePathText : {overridePathText}");

					FDebug.Log($"Before apply override: {action.bindings[bindingIndex].path}");
					var bindingOverride = new InputBinding { overridePath = bindingObject.Value[i][0], name = bindingObject.Value[i][1]};


					FDebug.Log($"bindingIndex : {bindingIndex} \nbindingOverride : {bindingOverride.name}, {bindingOverride.path}");

					action.Disable();
					action.Enable();
					action.ApplyBindingOverride(bindingIndex, bindingOverride);
					FDebug.Log($"After  apply override: {action.bindings[bindingIndex].path}");
				}
				else
				{
					FDebug.LogWarning($"bindingIndext값에 문제가 발생했습니다. \nactionName : {bindingObject.Value[i][1]}, key : {bindingObject.Value[i][0]}");
				}
			}
		}

		pendingBindings.Clear();
	}
}

	