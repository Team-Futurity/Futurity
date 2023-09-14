using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlCommand
{
	void Init();

	void Run();
	
	void Stop();
}
