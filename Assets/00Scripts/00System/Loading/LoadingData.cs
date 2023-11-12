using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Loading Data", menuName = "Loading/Data", order = 0)]
public class LoadingData : ScriptableObject
{
	public Sprite cctveImage;
	
	[TextArea]
	public string cctvText;
}
