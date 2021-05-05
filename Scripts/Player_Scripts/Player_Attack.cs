using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private WeaponManager weapon_Manager;
    public float fireRate = 1f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;

    private GameObject auxiliar;
    private GameObject auxiliar2;
    private Camera mainCam;
    private GameObject crossHair;
    private bool Zoomed;
    private bool is_Aiming;
    private float current_Time = 2;

    [SerializeField]
    private GameObject arrow_Prefab;

    [SerializeField]
    private Transform bullet_Start_Pos;

    private void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();

        auxiliar = GameObject.FindWithTag(Weapon_Tags.LOOK_ROOT);
        auxiliar2 = GameObject.FindWithTag(Weapon_Tags.ZOOM_CAMERA);
        zoomCameraAnim = auxiliar2.GetComponent<Animator>();
        //find the animator from FP_camera
        crossHair = GameObject.FindWithTag(Weapon_Tags.CROSSHAIR);
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && weapon_Manager.GetCurrentSelectedWeapon().isActiveAndEnabled)
        {
            Weapon_Shoot();
            Zoom_IN_OUT();
        }
    }

    void Weapon_Shoot()
    {
        //print(fireRate + " >= " + current_Time);
        current_Time += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && current_Time >= fireRate)
        {
            current_Time = 0;
            if (weapon_Manager.GetCurrentSelectedWeapon().bullet_Type == WeaponBulletType.BULLET)
            {
                weapon_Manager.GetCurrentSelectedWeapon().Shoot_Anim();
                BulletFire();
                gameObject.GetComponent<Enemy_Notifier>().Notify(transform.position, 80f);
            }
            else
            {
                if (is_Aiming)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().Shoot_Anim();
                    Throw_Arrow();
                }// throw projectile
            }
        }
    } //Weapon shoot function

    void Zoom_IN_OUT()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if(Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);
                crossHair.SetActive(false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);
                crossHair.SetActive(true);
            }
        } // weapons with zoom aim

        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }
        } // weaopns with self aim
    } //zoom functions

    void Throw_Arrow()
    {
        GameObject arrow = Instantiate(arrow_Prefab);
        arrow.transform.position = bullet_Start_Pos.position;

        arrow.GetComponent<ArowScript>().Lounch(mainCam);
    }// throw arrow

    void BulletFire()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            if(hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<Health_Script>().ApplyDamage(damage);
                print(hit.transform.tag);
            }
        }
    }// fire bullet
}
