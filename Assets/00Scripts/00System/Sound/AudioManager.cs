using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	private List<EventInstance> eventInstances = new List<EventInstance>();
	private EventInstance ambientInstance;


	public void PlayOneShot(EventReference sound, Vector3 worldPos)
	{
		RuntimeManager.PlayOneShot(sound, worldPos);
	}

	public EventInstance CreateInstance(EventReference eventReference)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(eventInstance);
		return eventInstance;
	}

	public void SetAmbientSound(EventReference ambientReference)
	{
		ambientInstance = CreateInstance(ambientReference);
		ambientInstance.start();
	}

	public void CleanUp()
	{
		foreach (EventInstance eventInstance in eventInstances)
		{
			eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			eventInstance.release();
		}

		eventInstances.Clear();
	}
}
