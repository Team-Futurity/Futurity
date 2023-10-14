using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingTest : MonoBehaviour
{
    public GameObject dummyPrefab;
    public GameObject dummyParent;
    private ObjectPoolManager<FloatingText> a;
    private void Start()
    {
        a = new ObjectPoolManager<FloatingText>(dummyPrefab, dummyParent);
        a.ActiveObject();
        FloatingText obj = a.ActiveObject();
        a.DeactiveObject(obj);
        a.ActiveObject();
    }
}
