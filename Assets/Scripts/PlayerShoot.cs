/*using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
    [SerializeField]
    private PlayerWeapons weapon = new Pistol();

    [SerializeField]
    private GameObject weaponGraphics;

    [SerializeField]
    private string weaponLayername = "Weapon";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;


    void Start ()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No cam referenced");
            this.enabled = false;
        }
        weapon.initializePistol();
        weaponGraphics.layer = LayerMask.NameToLayer(weaponLayername);
    }

    void Update()
    {
        if (PauseMenu.IsOn)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot ()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.getRng(), mask) )
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.getDmg());
            }
        }
    }

    [Command]
    void CmdPlayerShot (string _playerID, int _damage)
    {
        Debug.Log(_playerID + "has been shot");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }

}*/
