using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Notifier:MonoBehaviour
{
    private float MinPos = -5f;
    private float MaxPos = 5f;

    public GameObject[] enemies;
    public void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
    }

    public void Notify(Vector3 obj_Pos, float notify_Distance)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            try
            {
                enemies[i].GetComponent<EnemyController>().Sound_Notify(obj_Pos, notify_Distance);
            }
            catch
            {
                //Debug.Log("Turret notified");
            }
        }
    } // notify enemy

    public void TurretNotifyer(Vector3 obj_Pos, float notify_Distance)
    {
        obj_Pos.x += Random.Range(MinPos, MaxPos);
        obj_Pos.z += Random.Range(MinPos, MaxPos);
        for (int i = 0; i < enemies.Length; i++)
        {
            try
            {
                print(obj_Pos);
                if (enemies[i].GetComponent<EnemyController>().Enemy_State == EnemyState.PATROL)
                {
                    enemies[i].GetComponent<EnemyController>().Sound_Notify(obj_Pos, notify_Distance);
                }
            }
            catch
            {
                Debug.Log("Turret");
            }
        }
    } // notify enemy when the source is turret
}
