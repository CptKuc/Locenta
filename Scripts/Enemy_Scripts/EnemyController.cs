using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    GO_TO_SOUND,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    protected EnemyAnimator enemy_Anim;
    protected NavMeshAgent nav_Agent;

    protected EnemyState enemy_State;

    public float walk_Speed = 1.5f;
    public float run_Speed = 6f;
    public float going_Speed = 3f;

    public float target_Accuracy = 3f;
    public float chase_Distance = 8f;
    protected float current_Chase_Distance;
    public float attack_Distance = 2.2f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radiu_Max = 60f;
    public float patrol_For_This_Time = 15f;
    public float stay_For_This_Time = 10f;
    public float patrol_Timer;
    //public float wait_Before_Attack = 2f;
    protected float attack_Timer;

    protected Transform target;
    protected Vector3 temporar_Destination;

    public GameObject attack_Point;

    protected Enemy_Audio enemy_Audio;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        nav_Agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
        enemy_Audio = GetComponentInChildren<Enemy_Audio>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy_State = EnemyState.PATROL;
        patrol_Timer = patrol_For_This_Time;
        //attack_Timer = wait_Before_Attack;
        current_Chase_Distance = chase_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        //print(Vector3.Distance(transform.position, target.position));
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

    public void Sound_Notify(Vector3 sound_Position, float sound_Distance)
    {
        if (Vector3.Distance(transform.position, sound_Position) < sound_Distance
            && enemy_State != EnemyState.CHASE && enemy_State != EnemyState.ATTACK)
        {
            temporar_Destination = sound_Position;
            enemy_State = EnemyState.GO_TO_SOUND;
        }
    } // notify the enemy of the possition of hte sound

    void Go_To_Sound_Position(Vector3 sound_Position)
    {
        stay_For_This_Time = 1f;
        patrol_For_This_Time = 6f;
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
            //play spoted audios
            enemy_Audio.StopSound();
            enemy_Audio.PlayScreamSoud();
        }

        if (Vector3.Distance(transform.position, sound_Position) < target_Accuracy)
        { //enemy get close enough to the target
            enemy_State = EnemyState.PATROL;
            patrol_Timer = patrol_For_This_Time;
            enemy_Audio.StopSound();

            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
    } //go to sound positino

    void Patrol()
    {
        nav_Agent.isStopped = false;
        nav_Agent.speed = walk_Speed;
        patrol_Timer += Time.deltaTime;

        if (patrol_Timer < stay_For_This_Time)
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

    void Chase()
    {
        stay_For_This_Time = 1f;
        patrol_For_This_Time = 6f;
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

        enemy_Anim.Attack();

        if (Vector3.Distance(transform.position, target.position) >
            attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }

    protected void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radiu_Max);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        nav_Agent.SetDestination(navHit.position);
    }

    void Turn_ON_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_OFF_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State 
    {
        get; set;

    }
}
