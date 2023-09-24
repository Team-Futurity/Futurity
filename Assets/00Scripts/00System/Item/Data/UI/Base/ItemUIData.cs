using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI Data", menuName = "UI/Data", order = 0)]
public class ItemUIData : ScriptableObject
{
	[field: SerializeField] public string ItemName { get; private set; }
	[field: SerializeField] public string ItemDescription { get; private set; }
	
	[field: SerializeField] public Sprite ItemSprite { get; private set; }
}
