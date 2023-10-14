using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour, PoolingObject
{
    public TMP_Text text;
    public bool isUp;
    public float existanceTime;
    public float moveSpeed;

    [SerializeField]  private bool isActive;
    private float currentTime;
    private ObjectPoolManager<FloatingText> poolManager;

    public void Initialize(OBJPoolParent objPM)
    {
        poolManager = (ObjectPoolManager<FloatingText>)objPM;
    }

    public void ActiveObj()
    {
        isActive = true;
    }

    public void DeactiveObj()
    {
        isActive = false;
        currentTime = 0;
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 pos = gameObject.transform.position;
            float yDir = isUp ? 1 : -1;
            currentTime += Time.deltaTime;
            gameObject.transform.position = new Vector3(pos.x, pos.y + yDir * moveSpeed * Time.deltaTime, pos.z);

            if(currentTime > existanceTime)
            {
                // 비활성화
                poolManager.DeactiveObject(this);
            }
        }
    }

    
}
