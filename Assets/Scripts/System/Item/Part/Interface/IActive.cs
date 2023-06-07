using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActive
{
	public void RunActive(PlayerController pc);

	public void StopActive();
}