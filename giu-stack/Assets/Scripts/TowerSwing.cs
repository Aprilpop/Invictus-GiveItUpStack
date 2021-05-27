using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSwing : MonoBehaviour
{
    /// <summary>
    /// 振幅
    /// </summary>
    public float amplitude;
    /// <summary>
    /// 频率
    /// </summary>
    public float frequency;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        posOffset = transform.position;
    }

    private void Update()
    {
        tempPos = posOffset;
        tempPos.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
