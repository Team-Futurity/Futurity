using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeableStatusListDictionary : SerializableDictionary<string, StatusList>
{

}

[CreateAssetMenu(fileName = "Test", menuName = "TEST/TEST", order = 0)]
public class UnitStatus : ScriptableObject
{
	[SerializeField]
	public SerializeableStatusListDictionary test23;

	public float currentHp = 200f;
	
	public float maxHp = 200f;
	
	public float speed = 3f;
	
	public float attack = 20f;
	
	public float defence = 5f;
	
	[Range(0, 1)]
	public float criticalChance = 0f;
	
	public float criticalDamageMultiplier = 0f;
}
