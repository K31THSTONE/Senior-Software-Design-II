using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    public GameObject weaponOnGround;
    public WeaponManager weaponManager;
    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter(Collider _collider)
    {
        if(_collider.gameObject.tag == "Player"){
            //create playerweapon from gameobject then pass playerweapon object to equip weapon
            weaponManager = GetComponent<WeaponManager>();
            Submachine weapon = new Submachine();
            weapon.initializeSubmachine(weapon);
            //weaponManager.equipWeapon(weapon);
        }
    }
}
