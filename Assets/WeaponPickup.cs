using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter(Collider _collider)
    {
        if(_collider.gameObject.tag == "Player"){
            //create playerweapon from gameobject then pass playerweapon object to equip weapon
            WeaponManager weaponManager = GetComponent<WeaponManager>();
            PlayerWeapons weapon = new PlayerWeapons();
            weapon.initializeSubmachine();
            weaponManager.EquipWeapon(weapon);
        }
    }
}
