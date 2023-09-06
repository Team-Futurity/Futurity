using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSErrorData
{
	public Color Color { get; set; }

	public CSErrorData()
	{
		GenerateRandomColor();
	}

	private void GenerateRandomColor()
	{
		Color = new Color32(
			(byte)Random.Range(128, 256),
			(byte)Random.Range(32, 169),
			(byte)Random.Range(32, 169),
			255
		);
	}
}
