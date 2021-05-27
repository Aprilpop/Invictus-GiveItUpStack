using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    public Vector3 offset;
    Vector3 swing;
    public float offsetY;
    public float smoothnes;

    Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();

        if (ProfileManager.Instance.Ratio < 1.5)
            camera.fieldOfView = 48;
        else if (ProfileManager.Instance.Ratio > 2.0)
            camera.fieldOfView = 73;

            transform.position = offset;
    }

    private void Update()
    {

        if (GameLogic.Instance.currentStack)
        {
            Vector3 start = new Vector3(transform.position.x, GameLogic.Instance.currentStack.transform.position.y + offsetY, transform.position.z);
            Vector3 smooth = Vector3.Lerp(transform.position, start, smoothnes * Time.deltaTime);
            transform.position = smooth;
        }
        if (GameLogic.Instance.previousStack)
        {            
            // swing = new Vector3(GameLogic.Instance.Swing(), transform.position.y, transform.position.z);
            // transform.position = swing = new Vector3(transform.position.x, transform.position.y, transform.position.z);;
        } 
    }

}
