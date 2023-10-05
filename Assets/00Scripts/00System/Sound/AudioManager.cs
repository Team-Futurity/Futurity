using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private Transform playerTransform;

	[Header("Volume")]
	[SerializeField, ReadOnly] private float masterVolume = 1f;
	[SerializeField, ReadOnly] private float bcakgroundMusicolume = 1f;
	[SerializeField, ReadOnly] private float ambientVolume = 1f;
	[SerializeField, ReadOnly] private float objectVolume = 1f;
	[SerializeField, ReadOnly] private float userInterfaceVolume = 1f;

	private Dictionary<BusType, Bus> busDictionary = new Dictionary<BusType, Bus>();

	private Bus masterBus;
	private Bus backgroundMusicBus;
	private Bus ambientBus;
	private Bus objectBus;
	private Bus userInterfaceBus;

	private List<EventInstance> eventInstances = new List<EventInstance>();
	private EventInstance ambientInstance;
	private EventInstance backgroundMusicInstance;

	protected override void Awake()
	{
		base.Awake();

		masterBus			= RuntimeManager.GetBus("bus:/");
		backgroundMusicBus	= RuntimeManager.GetBus("bus:/BGM");
		ambientBus			= RuntimeManager.GetBus("bus:/AMB");
		objectBus			= RuntimeManager.GetBus("bus:/Object");
		userInterfaceBus	= RuntimeManager.GetBus("bus:/UI");

		busDictionary.Add(BusType.Master, masterBus);
		busDictionary.Add(BusType.BGM, backgroundMusicBus);
		busDictionary.Add(BusType.AMB, ambientBus);
		busDictionary.Add(BusType.Object, objectBus);
		busDictionary.Add(BusType.UI, userInterfaceBus);
	}

	public void PlayOneShot(EventReference sound, Vector3 worldPos)
	{
		if (sound.IsNull) { return; }

		RuntimeManager.PlayOneShot(sound, worldPos);
	}

	public EventInstance CreateInstance(EventReference eventReference)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(eventInstance);
		return eventInstance;
	}

	public float GetVolume(BusType busType)
	{
		Bus bus;

		if (busDictionary.TryGetValue(busType, out bus)) { FDebug.LogWarning($"This Bus is not Exist : {busType}", GetType()); return -1; }

		bus.getVolume(out float soundVolume);

		return soundVolume;
	}

	public void SetVolume(BusType busType, float volume)
	{
		float soundVolume = Mathf.Clamp(volume, 0f, 1f);
		Bus bus;

		if(busDictionary.TryGetValue(busType, out bus)) { FDebug.LogWarning($"This Bus is not Exist : {busType}", GetType()); return; }

		bus.setVolume(soundVolume);
	}

	public void RunAmbientSound(EventReference ambientReference)
	{
		if (ambientReference.IsNull) { return; }

		
		//ambientInstance.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
		if (CheckPlayerTransform())
		{
			ambientInstance = CreateInstance(ambientReference);
			eventInstances.Add(ambientInstance);
			RuntimeManager.AttachInstanceToGameObject(ambientInstance, playerTransform);
			ambientInstance.start();
		}
	}

	public void RunBackgroundMusic(EventReference backgroundMusicReference)
	{
		if (backgroundMusicReference.IsNull) { return; }

		backgroundMusicInstance = CreateInstance(backgroundMusicReference);
		eventInstances.Add(backgroundMusicInstance);
		backgroundMusicInstance.start();
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

	private bool CheckPlayerTransform()
	{
		if (playerTransform != null) { return true; }

		return SetPlayerTransform();
	}

	private bool SetPlayerTransform()
	{
		playerTransform = FindObjectOfType<PlayerController>().transform;

		if(playerTransform == null) { FDebug.LogWarning("Player is not Exist", GetType()); return false; }

		return true;
	}
}
