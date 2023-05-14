using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBindingManager : Singleton<KeyBindingManager>
{
	private List<KeyBinding> keyBindings;

	protected override void Awake()
	{
		base.Awake();
		keyBindings = new List<KeyBinding>();
	}

	public void RegisterKeyBinding(KeyBinding keyBinding)
	{	
		keyBindings.Add(keyBinding);
	}

	public bool CheckIfKeyIsUsed(InputActionReference actionReference, string newBinding)
	{
		foreach (KeyBinding keyBinding in keyBindings)
		{
			if (keyBinding != null && keyBinding.actionReference != actionReference)
			{
				string currentBinding = keyBinding.actionReference.action.GetBindingDisplayString(0);
				if (currentBinding == newBinding)
				{
					FDebug.Log("중복 키 바인딩");
					return true;
				}
			}
		}
		return false;
	}
}