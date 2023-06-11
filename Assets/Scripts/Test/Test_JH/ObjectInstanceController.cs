using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectInstanceController : MonoBehaviour
{
	[SerializeField]
	List<GameObject> objects = new List<GameObject>();


	public void instanceObject(int number)
	{
		Instantiate(objects[number]);
	}

	public void destroyObject(GameObject destroyObject)
	{
		Destroy(destroyObject);
	}
}
