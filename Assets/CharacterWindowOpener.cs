using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterWindowOpener : MonoBehaviour
{
	[SerializeField] 
	private InputActionReference charactorInput;

	[SerializeField]
	private GameObject characterWindow;


	private void Start()
	{
		charactorInput.action.performed += _ => CharacterWindowOpen();

		charactorInput.action.Enable();
	}

	private void CharacterWindowOpen()
	{
		Instantiate(characterWindow);
	}
}
