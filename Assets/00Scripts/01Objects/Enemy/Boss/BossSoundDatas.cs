using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Diagnostics;

[CreateAssetMenu(fileName = "BossSoundDatas", menuName = "ScriptableObject/Boss/BossSoundDatas")]
public class BossSoundDatas : ScriptableObject
{
	public List<BossSoundData> soundDatas = new List<BossSoundData>();

	public EventReference GetSoundReference(BossState state, SoundType type)
	{
		EventReference sound = new EventReference();

		foreach (var data in soundDatas)
			if (data.state == state && data.soundType == type)
				sound = data.soundReference;

		return sound;
	}
}
