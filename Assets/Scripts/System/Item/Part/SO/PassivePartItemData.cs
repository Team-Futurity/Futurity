using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PassivePartItemData", menuName ="PartData/Passive", order = 0)]
public class PassivePartItemData : PartItemData
{
	// Grade
	[field: SerializeField] public PassivePartGrade PartGrade { get; private set; }
	
	// Activation 
	[field: SerializeField] public float PartActivation { get; private set; }

	// AbilityType 
	[field: SerializeField] public PassivePartAbility PartAbility { get; private set; }

	// AbilityCount
	[field: SerializeField] public float PartAbilityCount { get; private set; }

	// AbilityDuration
	[field: SerializeField] public float PartAbilityDuration { get; private set; }


}
