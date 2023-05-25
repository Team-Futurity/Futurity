using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SceneKeyData", menuName = "SceneKeyData/KeyData", order = 0)]
public class SceneKeyData : ScriptableObject
{
	public string sceneName;
	public string chapterName;
	public string incidentName;
}
