using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Health_Script : MonoBehaviour
{

    private EnemyAnimator enemy_Anim;
    private NavMeshAgent nav_Agent;
    private EnemyController enemy_Controller;
    public float health = 100f;
    public bool is_Player, is_Enemy, is_Turret;
    private bool is_Dead;
    private TurretScript turret_Controller;

    private GameObject explosion;

    private Enemy_Audio enemy_Audio;

    private Player_Stats player_Stats;

    void Awake ()
    {
        if (is_Enemy)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            nav_Agent = GetComponent<NavMeshAgent>();

            enemy_Audio = GetComponentInChildren<Enemy_Audio>();
            //get enemy audio
        }

        if (is_Turret)
        {
            turret_Controller = GetComponent<TurretScript>();
            enemy_Audio = GetComponentInChildren<Enemy_Audio>();
            explosion = gameObject.transform.GetChild(4).gameObject;
        }

        if (is_Player)
        {
            player_Stats = GetComponent<Player_Stats>();
            //get player stats
        }
    }

    public void ApplyDamage(float damage)
    {
        if (is_Dead)
        {
            return;
        }

        health -= damage;
        if(is_Player)
        {
            player_Stats.DisplayHealthStats(health);
            //player_Stats.DisplayStaminaStats(stamina);
            //display UI
        }

        if(is_Enemy)
        {
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50000f;
            }
        }

        if(health <= 0)
        {
            PlayerDied();
            is_Dead = true;
        }
    }
    void PlayerDied()
    {
        if (is_Enemy)
        {
            nav_Agent.velocity = Vector3.zero;
            nav_Agent.isStopped = true;
            enemy_Controller.enabled = false;
            enemy_Anim.Dead();
            
            StartCoroutine(DeadSound());
        }

        if (is_Turret)
        {
            turret_Controller.enabled = false;
        }

        if (is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for (int i = 0; i < enemies.Length; i++)
            {
                if (is_Enemy)
                {
                    enemies[i].GetComponent<EnemyController>().enabled = false;
                    enemies[i].GetComponentInChildren<AudioSource>().enabled = false;
                }
                if (is_Turret)
                {
                    enemies[i].GetComponent<TurretScript>().enabled = false;
                    enemies[i].GetComponentInChildren<AudioSource>().enabled = false;
                }
            }

            // call enemy manager

            GetComponent<Player_Movement>().enabled = false;
            GetComponent<Player_Attack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }

        if (tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        }
        else if (is_Enemy)
        {
            Invoke("TurnOffGameObject", 5f);
        }
        else if (is_Turret)
        {
            Invoke("Explosion", 3f);
            Invoke("TurnOffGameObject", 7f);
        }
    }

    void Explosion()
    {
        explosion.SetActive(true);
        gameObject.GetComponent<Enemy_Notifier>().TurretNotifyer(transform.position, 400f);
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemy_Audio.PlayDeadSound();
    }
}
