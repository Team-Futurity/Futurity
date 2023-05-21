using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI Data", menuName = "UI/Data", order = 0)]
public class ItemUIData : ScriptableObject
{
	[field: SerializeField] public string itemName { get; private set; }
	[field: SerializeField] public string itemDescription { get; private set; }
	
	[field: SerializeField] public Sprite itemSprite { get; private set; }
}
