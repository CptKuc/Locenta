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
    }

    public void LounchForEnemy(Vector3 direction)
    {
        myBody.velocity = direction * speed * 2;
        transform.LookAt(transform.position + myBody.velocity);
        transform.Rotate(90, 0, 0);
    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider target)
    {
        print(target.tag);
        if (target.tag == Tags.ENEMY_TAG)
        {
            print(target.tag);
            //target.GetComponent<Health_Script>().ApplyDamage(damage);
            target.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
            try
            {
                target.transform.GetChild(2).gameObject.GetComponent<Renderer>().enabled = true;
            }
            catch
            {
                try
                {
                    target.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(4).GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
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
        }
        gameObject.SetActive(false);
    }
}
