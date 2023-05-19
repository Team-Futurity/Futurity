using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartBehaviour : MonoBehaviour
{
	[field:SerializeField] public PartItemData itemData { get; private set; }

}
