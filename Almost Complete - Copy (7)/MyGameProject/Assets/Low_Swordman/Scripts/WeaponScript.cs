using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int totalWeapon;
    public int currentWeaponIndex;

    public GameObject[] weapons;
    public GameObject weaponHolder;
    public GameObject currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
        totalWeapon = weaponHolder.transform.childCount;
        weapons = new GameObject[totalWeapon];

        for (int i = 0; i< totalWeapon; i++)
        {
            weapons[i] = weaponHolder.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }

        weapons[0].SetActive(true);
        currentWeapon = weapons[0];
        currentWeaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && currentWeaponIndex == 1)
        {
            weapons[currentWeaponIndex].SetActive(false);
            currentWeaponIndex -= 1;
            weapons[currentWeaponIndex].SetActive(true);
        }
        if (Input.GetButtonDown("Fire1") && currentWeaponIndex == 0)
        {
            weapons[currentWeaponIndex].SetActive(false);
            currentWeaponIndex += 1;
            weapons[currentWeaponIndex].SetActive(true);
        }
    }
}
