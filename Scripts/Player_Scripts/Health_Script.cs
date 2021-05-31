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
        } // take components needed for enemy

        if (is_Turret)
        {
            turret_Controller = GetComponent<TurretScript>();
            enemy_Audio = GetComponentInChildren<Enemy_Audio>();
            explosion = gameObject.transform.GetChild(3).gameObject;
        } // take components needed for turret

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
        } // damage the enemy and make him go after the player

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
            // stop the enemy

            nav_Agent.isStopped = true;
            // stop navMeshAgent

            enemy_Controller.enabled = false;
            // deactivate controller

            enemy_Anim.Dead();
            // play animation
            
            StartCoroutine(DeadSound());

        } // kill an enemy

        if (is_Turret)
        {
            turret_Controller.enabled = false;
            // stop the turret from doing enything
        }

        if (is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            // make vector with all enemies and turrets

            for (int i = 0; i < enemies.Length; i++)
            {
                if (is_Enemy)
                {
                    enemies[i].GetComponent<EnemyController>().enabled = false;
                    enemies[i].GetComponentInChildren<AudioSource>().enabled = false;
                } // dissable enemies

                if (is_Turret)
                {
                    enemies[i].GetComponent<TurretScript>().enabled = false;
                    enemies[i].GetComponentInChildren<AudioSource>().enabled = false;
                } // disable turrets

            } // deactivate all enemies

            GetComponent<Player_Movement>().enabled = false;
            GetComponent<Player_Attack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
            // stop player functions and deactivate weapon

        }

        if (tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        } // restart the game

        else if (is_Enemy)
        {
            Invoke("TurnOffGameObject", 5f);
        } // make enemy disapear


        else if (is_Turret)
        {
            Invoke("Explosion", 1f);
            Invoke("TurnOffGameObject", 4f);
        } // make turret explode and disapear
    }

    void Explosion()
    {
        explosion.SetActive(true);
        // start turret explosion

        gameObject.GetComponent<Enemy_Notifier>().TurretNotifyer(transform.position, 100f);
        // notify all enemies in 100 radius

        gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = false;
        // deactivate turret render

        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        // deactivate turret sound
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } // restart game by reloading the current scene

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
