using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    private float x, y;
    public float rotation_SpeedX = 2f;
    public float rotation_SpeedY = 5f;

    void Start()
    {
        x = 0;
        y = 0;
    }
    // Update is called once per frame
    void Update()
    {
        x += rotation_SpeedX * Time.deltaTime;
        y += rotation_SpeedY * Time.deltaTime;
        transform.rotation = Quaternion.Euler(x, y, 0);
    }
}
