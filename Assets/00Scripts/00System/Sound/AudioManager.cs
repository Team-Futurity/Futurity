using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private Transform playerTransform;

	private Dictionary<BusType, Bus> busDictionary = new Dictionary<BusType, Bus>();

	private Bus masterBus;
	private Bus backgroundMusicBus;
	private Bus ambientBus;
	private Bus objectBus;
	private Bus userInterfaceBus;

	private List<EventInstance> eventInstances = new List<EventInstance>();
	private List<EventInstance> deletingInstances = new List<EventInstance>();
	public EventInstance ambientInstance { get; private set; }
	public EventInstance backgroundMusicInstance { get; private set; }

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


	#region Volume
	public float GetVolume(BusType busType)
	{
		if(busType == BusType.None) { return -1f; }

		Bus bus;
		if (!busDictionary.TryGetValue(busType, out bus)) { FDebug.LogWarning($"This Bus is not Exist : {busType}", GetType()); return -1f; }

		bus.getVolume(out float soundVolume);

		return soundVolume;
	}

	public void SetVolume(BusType busType, float volume)
	{
		if (busType == BusType.None) { return; }

		float soundVolume = Mathf.Clamp(volume, 0f, 1f);
		
		Bus bus;
		if(!busDictionary.TryGetValue(busType, out bus)) { FDebug.LogWarning($"This Bus is not Exist : {busType}", GetType()); return; }

		bus.setVolume(soundVolume);
	}
	#endregion

	#region BackgroundSounds
	#region RunBacgroundSounds
	public void RunAmbientSound(EventReference ambientReference)
	{
		if (ambientReference.IsNull) { return; }

		
		//ambientInstance.set3DAttributes(RuntimeUtils.To3DAttributes(cameraTransform));
		if (CheckPlayerTransform())
		{
			StopAmbientSound();

			ambientInstance = CreateInstance(ambientReference);
			eventInstances.Add(ambientInstance);
			RuntimeManager.AttachInstanceToGameObject(ambientInstance, playerTransform);
			ambientInstance.start();
		}
	}

	public void RunBackgroundMusic(EventReference backgroundMusicReference)
	{
		if (backgroundMusicReference.IsNull) { return; }

		StopBackgroundMusic();

		backgroundMusicInstance = CreateInstance(backgroundMusicReference);
		eventInstances.Add(backgroundMusicInstance);
		backgroundMusicInstance.start();
	}
	#endregion

	#region SetParameterBackgroundSounds
	public void SetParameterForBackgroundMusic(ParamRef param, bool ignoreSeekSpeed = false)
	{
		if (!backgroundMusicInstance.isValid()) { return; }
		if (param == null) { return; }

		var result = backgroundMusicInstance.setParameterByName(param.Name, param.Value, ignoreSeekSpeed);

		FDebug.Log("BGM : " + result);
	}

	public void SetParameterForAmbientSound(ParamRef param, bool ignoreSeekSpeed = false)
	{
		if (!ambientInstance.isValid()) { return; }
		if (param == null) { return; }

		var result = ambientInstance.setParameterByName(param.Name, param.Value, ignoreSeekSpeed);

		FDebug.Log("AMB : " + result);
	}
	#endregion

	#region StopBackgroundSounds
	public void StopBackgroundMusic(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (!backgroundMusicInstance.isValid()) { return; }

		backgroundMusicInstance.stop(stopMode);
		backgroundMusicInstance.release();
	}

	public void StopAmbientSound(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (!ambientInstance.isValid()) { return; }

		ambientInstance.stop(stopMode);
		ambientInstance.release();
	}
	#endregion
	#endregion

	public void RegistDeletingInstance(EventInstance instance)
	{
		deletingInstances.Add(instance);
	}

	public void CleanUp()
	{
		foreach(EventInstance instance in deletingInstances)
		{
			if (!instance.isValid()) { continue; }

			instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			instance.release();
		}

		foreach (EventInstance eventInstance in eventInstances)
		{
			if(eventInstance.handle == backgroundMusicInstance.handle || eventInstance.handle == ambientInstance.handle) { continue; }

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
