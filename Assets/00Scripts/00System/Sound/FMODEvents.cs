using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents: MonoBehaviour
{
	public EventReference amb;
	private EventInstance ambInstance;

	public static FMODEvents instance { get; private set; }

	private void Awake()
	{
		if (instance == null)
		{ instance = this; }
	}

	private void Start()
	{
		/*ambInstance = AudioManager.Instance.CreateInstance(amb);
		ambInstance.start();*/

		AudioManager.Instance.PlayOneShot(amb, Vector3.zero);
	}

	private void OnDestroy()
	{
		//ambInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
}
