using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Script : MonoBehaviour
{
    public float damage = 2f;
    public float radius = 1f;
    public LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            hits[0].gameObject.GetComponent<Health_Script>().ApplyDamage(damage);
            //print("We touch: " + hits[0].gameObject.tag);
            gameObject.SetActive(false);
        }
    }
}
