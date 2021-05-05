using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponHandler[] weapons;

    private int current_Weapon;
    // Start is called before the first frame update
    void Start()
    {
        current_Weapon = 2;
        weapons[current_Weapon].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnWeapon(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            weapons[current_Weapon].gameObject.SetActive(false);
            current_Weapon = 2;
        }
    } //update

    void TurnOnWeapon(int weaponIndex)
    {
        if(current_Weapon == weaponIndex)
        {
            return;
        }
        weapons[current_Weapon].gameObject.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);
        current_Weapon = weaponIndex;
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[current_Weapon];
    }
}
