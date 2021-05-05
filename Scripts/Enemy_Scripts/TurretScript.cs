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
        turret_Charge = transform.GetChild(3).gameObject;
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
        //Tracking();
        AimFire();
        VerifyDistance();
    }

    void AimFire()
    {
        if (target)
        {
            aim.LookAt(target);
            aim.eulerAngles = new Vector3(aim.eulerAngles.x - 90, aim.eulerAngles.y, 0);
            head.rotation = Quaternion.Lerp(head.rotation, aim.rotation, Time.deltaTime * turn_Speed);
            Vector3 fwd = muzzle_Pos.TransformDirection(Vector3.forward);
            RaycastHit hit;
            Debug.DrawRay(muzzle_Pos.position, fwd * range, Color.red);

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
            }
            else
            {
                can_See = false;
            }
            if (Time.time >= next_Fire_Time && can_See == true)
            {
                Fire();
            }
            else 
            {
                turet_Animator.SetBool(Weapon_Tags.TURRET_FIRE, false);
            }
        }
    }

    void Fire()
    {
        next_Fire_Time = Time.time + reload_Time;
        turet_Animator.SetTrigger(Weapon_Tags.TURRET_FIRE);
        target.GetComponent<Health_Script>().ApplyDamage(damage);
        PlayChargeSound(true);
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
        }
        else
        {
            target = null;
            set_Target = false;
            PlayChargeSound(false);
        }
    }

    public void TurnOnFlash()
    {
        muzzle_Flash.SetActive(true);
        audio_Source.Play();
        gameObject.GetComponent<Enemy_Notifier>().TurretNotifyer(transform.position, 80f);
    }

    void PlayChargeSound(bool k)
    {
        if(k)
        {
            turret_Charge.GetComponent<AudioSource>().clip = charge_Sound;
            turret_Charge.GetComponent<AudioSource>().Play();
            Invoke("FireSound", 4f);
        }
        else
        {
            turret_Charge.GetComponent<AudioSource>().Stop();
        }
    }

    void FireSound()
    {
        turret_Charge.GetComponent<AudioSource>().clip = ready_Fire_Sound;
        turret_Charge.GetComponent<AudioSource>().Play();
    }

    public void TurnOffFlash()
    {
        muzzle_Flash.SetActive(false);
    }
}
