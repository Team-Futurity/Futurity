using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPassive
{
	public void Active(OriginStatus status);
	public void UnActive(OriginStatus status);

}
