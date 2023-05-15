using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class KeyBindingManager : MonoBehaviour
{
	private Dictionary<InputActionReference, string> pendingBindings = new Dictionary<InputActionReference, string>();

	public void RegisterPendingBinding(InputActionReference actionReference, string key)
	{
		pendingBindings[actionReference] = key;
	}

	public void ApplyBindings()
	{
		foreach (var binding in pendingBindings)
		{
			var action = binding.Key.action;
			action.ApplyBindingOverride(binding.Value);
		}

		pendingBindings.Clear();
	}
}

	