using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Vector3 rotate;

    public float amplitude;
    public float frequency;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        transform.Rotate(rotate);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.deltaTime * Mathf.PI *frequency) * amplitude;

        transform.position = tempPos;
    }          
}
