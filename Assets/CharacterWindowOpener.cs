using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterWindowOpener : MonoBehaviour
{
	[SerializeField]
	private WindowOpenController windowOpenController;

	private void Start()
	{
		windowOpenController.WindowOpen();
	}
}
