using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public Transform target, aim, head;
    public float reload_Time = 5f, turn_Speed = 2f, fire_Pause_Time = 0.25f, range = 10f;
    public Transform muzzle_Pos;
    public bool can_See = false;

    [SerializeField]
    private GameObject muzzle_Flash;

    [SerializeField]
    private AudioClip charge_Sound;

    [SerializeField]
    private AudioClip ready_Fire_Sound;

    private float next_Fire_Time;
    private Animator turet_Animator;
    private AudioSource audio_Source;
    public float damage = 50f;
    private GameObject player;
    private bool set_Target = false;

    private GameObject turret_Charge;

    void Awake()
    {
        turet_Animator = GetComponent<Animator>();
        audio_Source = GetComponent<AudioSource>();
        player = GameObject.FindWithTag(Tags.PLAYER_TAG);
        turret_Charge = transform.GetChild(2).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        turet_Animator = GetComponent<Animator>();
        audio_Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            AimFire();
        }
        VerifyDistance();
    }

    void AimFire()
    {
        aim.LookAt(target);
        aim.eulerAngles = new Vector3(aim.eulerAngles.x - 90, aim.eulerAngles.y, 0);
        head.rotation = Quaternion.Lerp(head.rotation, aim.rotation, Time.deltaTime * turn_Speed);
        // make the turret rotate after player

        Vector3 fwd = muzzle_Pos.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(muzzle_Pos.position, fwd * range, Color.red);
        // raycast for determine if tha player is hit

        if (Physics.Raycast(muzzle_Pos.position, fwd, out hit, range))
        {
            if (hit.collider.CompareTag(Tags.PLAYER_TAG))
            {
                can_See = true;
            }
            else
            {
                can_See = false;
            }
        } // determine if player is hit by raycast

        else
        {
            can_See = false;
        }

        if (Time.time >= next_Fire_Time && can_See == true)
        {
            Fire();
        } // fire if the player is hit by raycast and the turret is charged

        else 
        {
            turet_Animator.SetBool(Weapon_Tags.TURRET_FIRE, false);
        } // stop fire animation after fire
        
    } // method for aim and fire

    void Fire()
    {
        next_Fire_Time = Time.time + reload_Time;
        turet_Animator.SetTrigger(Weapon_Tags.TURRET_FIRE);
        // start fire animation

        target.GetComponent<Health_Script>().ApplyDamage(damage);
        // deal damage to the player

        PlayChargeSound(true);
        // play charge sound from the beginning after fire
    }

    void VerifyDistance()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            if (!set_Target)
            {
                next_Fire_Time = Time.time + reload_Time;
                target = player.transform;
                set_Target = true;
                PlayChargeSound(true);
            }
        } // set the player as target if in range

        else
        {
            target = null;
            set_Target = false;
            PlayChargeSound(false);
        } // reset target and stop sound

    } // Verify the distance between player and turret

    public void TurnOnFlash()
    {
        muzzle_Flash.SetActive(true);
        audio_Source.Play();
        gameObject.GetComponent<Enemy_Notifier>().TurretNotifyer(transform.position, 80f);
    } // turn on the particles and sound for turret shot

    void PlayChargeSound(bool k)
    {
        if(k)
        {
            turret_Charge.GetComponent<AudioSource>().clip = charge_Sound;
            turret_Charge.GetComponent<AudioSource>().Play();
            // play charge sound

            Invoke("FireSound", 4f);
            // invoke sound for turret ready to fire
        }
        else
        {
            turret_Charge.GetComponent<AudioSource>().Stop();
        } // stop the sound for turret charge
    }

    void FireSound()
    {
        turret_Charge.GetComponent<AudioSource>().clip = ready_Fire_Sound;
        turret_Charge.GetComponent<AudioSource>().Play();
    } // play sound for turret ready to fire

    public void TurnOffFlash()
    {
        muzzle_Flash.SetActive(false);
    } // turn off flash for turret shot
}
