using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBehaviour : MonoBehaviour
{
	// 이 곳에서 Buff의 Active를 진행한다.
	public virtual void Active() { }
	 
	public virtual void UnActive() { }

}
