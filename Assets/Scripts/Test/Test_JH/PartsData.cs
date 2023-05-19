using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PartsData", menuName = "PartsData")]
public class PartsData : ScriptableObject
{
	public string partsName;
	public Sprite partsSprite;
	public string partsMenual;
}