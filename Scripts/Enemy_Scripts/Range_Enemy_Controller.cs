using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range_Enemy_Controller : EnemyController
{
    [SerializeField]
    private GameObject arrow_Prefab;

    [SerializeField]
    private Transform bullet_Start_Pos;

    private float damping = 20f;

    [SerializeField]
    private Transform look_Rotarion;

    public float start_Going = 1f;
    protected float time_Passed = 0;

    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            enemy_Audio.PlayDefaultSound();
            Patrol();
        }

        if (enemy_State == EnemyState.GO_TO_SOUND)
        {
            enemy_Audio.PlayAwareSound();
            Go_To_Sound_Position(temporar_Destination);
        }

        if (enemy_State == EnemyState.CHASE)
        {
            enemy_Audio.PlayScreamSoud();
            Chase();
        }

        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        enemy_Anim.SetAttack(false); 
        nav_Agent.isStopped = false;
        nav_Agent.speed = walk_Speed;
        patrol_Timer += Time.deltaTime;

        if (patrol_Timer < stay_Fot_This_Time)
        {
            nav_Agent.isStopped = true;
        }

        if (patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        if (nav_Agent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;
            //play spoted audios
            enemy_Audio.StopSound();
            enemy_Audio.PlayScreamSoud();
        }
    }//patrol

    void Go_To_Sound_Position(Vector3 sound_Position)
    {
        time_Passed += Time.deltaTime;
        if (time_Passed < start_Going)
        {
            //nav_Agent.velocity = Vector3.zero;
            nav_Agent.isStopped = true;
            look_Rotarion.LookAt(sound_Position);
            transform.rotation = Quaternion.Lerp(transform.rotation, look_Rotarion.rotation, Time.deltaTime * damping);

            enemy_Anim.SetAttack(true);
        }
        if (time_Passed >= start_Going)
        {
            enemy_Anim.SetAttack(false);
            nav_Agent.isStopped = false;
            nav_Agent.speed = going_Speed;
            nav_Agent.SetDestination(sound_Position);

            if (nav_Agent.velocity.sqrMagnitude > 0)
            {
                enemy_Anim.Walk(true);
            }
            else
            {
                enemy_Anim.Walk(false);
            }

            if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
            {
                enemy_Anim.Walk(false);
                enemy_State = EnemyState.CHASE;
                time_Passed = 0;
                //play spoted audios
                enemy_Audio.StopSound();
                enemy_Audio.PlayScreamSoud();
            }

            if (Vector3.Distance(transform.position, sound_Position) < target_Accuracy)
            { //enemy get close enough to the target
                enemy_State = EnemyState.PATROL;
                time_Passed = 0;
                patrol_Timer = patrol_For_This_Time;
                enemy_Audio.StopSound();

                if (chase_Distance != current_Chase_Distance)
                {
                    chase_Distance = current_Chase_Distance;
                }
            }
        }
    }

    void Chase()
    {
        nav_Agent.isStopped = false;
        nav_Agent.speed = run_Speed;
        nav_Agent.SetDestination(target.position);

        if (nav_Agent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;
            
            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        { //player run away from enemy
            enemy_Anim.SetAttack(false);
            enemy_Anim.Run(false);
            temporar_Destination = target.position;
            enemy_State = EnemyState.GO_TO_SOUND;
            patrol_Timer = patrol_For_This_Time;
            enemy_Audio.StopSound();

            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
    }//chase

    void Attack()
    {
        nav_Agent.velocity = Vector3.zero;
        nav_Agent.isStopped = true;
        look_Rotarion.LookAt(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, look_Rotarion.rotation, Time.deltaTime * damping);

        enemy_Anim.SetAttack(true);
        attack_Timer = 0f;

        if (Vector3.Distance(transform.position, target.position) >
            attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
            enemy_Anim.SetAttack(false);
        }
    } //Attack

    void Throw_Arrow()
    {
        GameObject arrow = Instantiate(arrow_Prefab);
        arrow.transform.position = bullet_Start_Pos.position;
        arrow.GetComponent<ArowScript>().LounchForEnemy(transform.forward);
    }// throw arrow

    void PlayAudio()
    {
        enemy_Audio.PlayAttackSound();
    }
}
