using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArowScript : MonoBehaviour
{
    private Rigidbody myBody;

    public float speed = 8f;
    public float deactivate_Time = 15f;
    public float damage = 30f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivate_Time);
        // deactivate after deactivate_Time seconds
    }

    public void Lounch(Camera cameraMain)
    {
        myBody.velocity = cameraMain.transform.forward * speed;
        transform.LookAt(transform.position + myBody.velocity);
        transform.Rotate(90, 0, 0);
    } // lounch the projectile for player

    public void LounchForEnemy(Vector3 direction)
    {
        myBody.velocity = direction * speed * 2;
        transform.LookAt(transform.position + myBody.velocity);
        transform.Rotate(90, 0, 0);
    } // lounch the projectile for enemy

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    } // deactivate object

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.ENEMY_TAG)
        {
            target.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
            // the enemy armature

            try
            {
                target.transform.GetChild(2).gameObject.GetComponent<Renderer>().enabled = true;
                // the eyes for mele enemy
            }
            catch
            {
                try
                {
                    target.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(4).GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
                    // the rifel form soldier armature
                }
                catch
                {
                    //Debug.Log("Turret hit by plasma gun");
                }
            }
        }

        if (target.tag == Tags.PLAYER_TAG)
        {
            target.transform.GetComponent<Health_Script>().ApplyDamage(damage);
            // apply damage to player
        }
        gameObject.SetActive(false);
        // deactivate game object when it hits something
    }
}
