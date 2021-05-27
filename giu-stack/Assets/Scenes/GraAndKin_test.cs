
using UnityEngine;
using System.Collections;

public class GraAndKin_ts : MonoBehaviour
{
    public Rigidbody A, B;
    string str_AG = "";
    string str_AK = "";
    string str_BK = "";
    Vector3 v1, v2;
    void Start()
    {
        //为了更好的演示，将重力加速度降低
        Physics.gravity = new Vector3(0.0f, -0.5f, 0.0f);
        A.useGravity = false;
        B.useGravity = false;
        A.isKinematic = false;
        B.isKinematic = false;
        str_AG = "A开启重力感应";
        str_AK = "A关闭物理感应";
        str_BK = "B关闭物理感应";
        v1 = A.position;
        v2 = B.position;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10.0f, 10.0f, 200.0f, 45.0f), str_AG))
        {
            if (A.useGravity)
            {
                A.useGravity = false;
                str_AG = "A开启重力感应";
            }
            else
            {
                A.useGravity = true;
                str_AG = "A关闭重力感应";
            }
        }
        if (GUI.Button(new Rect(10.0f, 60.0f, 200.0f, 45.0f), str_AK))
        {
            if (A.isKinematic)
            {
                A.isKinematic = false;
                str_AK = "A关闭物理感应";
            }
            else
            {
                A.isKinematic = true;
                str_AK = "A开启物理感应";
            }
        }
        if (GUI.Button(new Rect(10.0f, 110.0f, 200.0f, 45.0f), str_BK))
        {
            if (B.isKinematic)
            {
                B.isKinematic = false;
                str_BK = "B关闭物理感应";
            }
            else
            {
                B.isKinematic = true;
                str_BK = "B开启物理感应";
            }
        }
        if (GUI.Button(new Rect(10.0f, 160.0f, 200.0f, 45.0f), "重置"))
        {
            A.position = v1;
            A.rotation = Quaternion.identity;
            B.position = v2;
            B.rotation = Quaternion.identity;
        }
    }
}
