using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents: MonoBehaviour
{
	public EventReference amb;
	private FMOD.Studio.EventInstance ambInstance;

	public static FMODEvents instance { get; private set; }

	private void Awake()
	{
		if (instance == null)
		{ instance = this; }
	}

	private void Start()
	{
		ambInstance = AudioManager.instance.CreateInstance(amb);
		ambInstance.start();
	}

	private void OnDestroy()
	{
		ambInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
}
