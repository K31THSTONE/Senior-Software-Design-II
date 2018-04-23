using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private PlayerWeapons primaryWeapon;

    private PlayerWeapons currentWeapon;
    private WeaponGraphics currentGraphics;

    private bool isReloading = false;

    void Start()
    {
        //will find current primary and place
        primaryWeapon.initializePistol();
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapons GetCurrentWeapon()
    {
        return currentWeapon;
    
    }

    public void EquipWeapon(PlayerWeapons _weapon)
    {
        currentWeapon = _weapon;
        //instantiates specific weapon to be held by the player within their hand, hence need for position and rotation
        GameObject _weaponInstance = (GameObject)Instantiate(_weapon.getGraphics(), weaponHolder.position, weaponHolder.rotation);
        //will make sure to follow weapon holder around
        _weaponInstance.transform.SetParent(weaponHolder);
        if (isLocalPlayer)
        {
            _weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

    public void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(Reload_Coroutine());
    }

    private IEnumerator Reload_Coroutine()
    {
        Debug.Log("Reloading..");
        //turn reload variable on
        isReloading = true;
        CmdOnReload();
        //pass in the current weapons reload time, which may differ per gun
        yield return new WaitForSeconds(currentWeapon.getReloadTime());
        currentWeapon.setBullets(currentWeapon.getMaxAmmo());
        //end reloading;
        isReloading = false;
    }


    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }


    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }
}