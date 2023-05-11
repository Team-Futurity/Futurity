using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	public float currentHp = 200f;
	
	public float maxHp = 200f;
	
	public float speed = 3f;
	
	public float attack = 20f;
	
	public float defence = 5f;
	
	[Range(0, 1)]
	public float criticalChance = .0f;
	
	public float criticalDamageMultiplier = .0f;


	// Only Player Status
	public float dashSpeed = .0f;

	// Only Monster Status
	// Coming soon

}
