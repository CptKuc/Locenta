using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    private GameObject left_Frame, right_Frame;
    private float opening_Time = 2f;
    private float current_Time;


    void Awake()
    {
        left_Frame = gameObject.transform.GetChild(0).gameObject;
        right_Frame = gameObject.transform.GetChild(1).gameObject;
    }
    
    public void Open()
    {
        if (current_Time < opening_Time)
        {
            left_Frame.transform.Translate(Vector3.forward * Time.deltaTime);
            right_Frame.transform.Translate(Vector3.back * Time.deltaTime);
            current_Time += Time.deltaTime;
        }
    }
}
