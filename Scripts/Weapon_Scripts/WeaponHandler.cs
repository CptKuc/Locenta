using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim
{
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType
{
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType
{
    BULLET,
    ARROW,
    NONE
}

public class WeaponHandler : MonoBehaviour
{
    private Animator anim;

    public WeaponAim weapon_Aim;

    [SerializeField]
    private GameObject muzzle_Flash;

    [SerializeField]
    private AudioSource shoot_Sound, reload_Sound;

    public WeaponFireType fire_Type;
    public WeaponBulletType bullet_Type;

    public GameObject attack_Point;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Shoot_Anim()
    {
        try
        {
            anim.SetTrigger(AnimationTags.SHOOT_TRIGGER);
        }
        catch
        {
            Debug.Log("No Weapon");
        }
    }

    public void Aim(bool canAim)
    {
        anim.SetBool(AnimationTags.AIM_PARAM, canAim);
    }

    void Turn_ON_MuzzleFlash()
    {
        muzzle_Flash.SetActive(true);
    }

    void Turn_OFF_MuzzleFlash()
    {
        muzzle_Flash.SetActive(false);
    }

    void Play_ShootSound()
    {
        shoot_Sound.Play();
    }

    void Play_ReloadSound()
    {
        reload_Sound.Play();
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

}
