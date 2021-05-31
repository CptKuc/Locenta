using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretExplosion : MonoBehaviour
{
    private Light explosion_Light;

    void Awake()
    {
        explosion_Light = gameObject.transform.GetComponentInChildren<Light>();
    }

    private void Start()
    {
        explosion_Light.intensity = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        explosion_Light.intensity -= Time.deltaTime * 10f;
    } // make the explosion light fade away
}
