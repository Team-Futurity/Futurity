using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBehaviour : MonoBehaviour
{
	// Enum 형식으로 Buff를 관리하는 것.

	public virtual void Active() { }
	 
	public virtual void UnActive() { }

}
