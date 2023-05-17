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
		pendingBindings[actionReference] = key;
	}

	public void RegisterMovePendingBinding(InputActionReference actionReference, string key, string actionName)
	{
		if (!pendingMultiBindings.ContainsKey(actionReference))
		{
			pendingMultiBindings[actionReference] = new List<string[]>();
		}

		pendingMultiBindings[actionReference].Add(new string[] { key, actionName });
	}

	public void ApplyBindings()
	{
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

					Debug.Log($"bindingIndex.path : {bindingObject.Value[i][0]} \nbindingIndex.name : {bindingObject.Value[i][1]}");
					string overridePathText = bindingObject.Value[i][0];
					Debug.Log($"overridePathText : {overridePathText}");

					Debug.Log($"Before apply override: {action.bindings[bindingIndex].path}");
					var bindingOverride = new InputBinding { overridePath = bindingObject.Value[i][0], name = bindingObject.Value[i][1]};


					Debug.Log($"bindingIndex : {bindingIndex} \nbindingOverride : {bindingOverride.name}, {bindingOverride.path}");

					action.Disable();
					action.Enable();
					action.ApplyBindingOverride(bindingIndex, bindingOverride);
					Debug.Log($"After  apply override: {action.bindings[bindingIndex].path}");
				}
				else
				{
					Debug.LogWarning($"bindingIndext값에 문제가 발생했습니다. \nactionName : {bindingObject.Value[i][1]}, key : {bindingObject.Value[i][0]}");
				}
			}
		}

		pendingBindings.Clear();
	}
}

	