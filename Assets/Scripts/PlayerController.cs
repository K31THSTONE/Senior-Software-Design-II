﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]

public class PlayerController : MonoBehaviour {
	
	//PlayerShootFields
	 private const string PLAYER_TAG = "Player";
    [SerializeField]
    private PlayerWeapons weapon = new PlayerWeapons();

    [SerializeField]
    private GameObject weaponGraphics;

    [SerializeField]
    private string weaponLayername = "Weapon";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
	//
	
	//PlayerSetupFields
	[SerializeField]
    Behaviour[] disabledComponents;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string dontDrawLayer = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;
    [SerializeField]
    GameObject playerUI;
    private GameObject playerUIInstance;

    Camera sceneCamera;
	//
	
	//PlayerMotorFields
	[SerializeField]
    private Camera cam;


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float camRotationX = 0f;
    private float currentCamRotationX = 0f;
    private Vector3 flyForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLim = 85f;
	
	 private Rigidbody rb;
	//
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSens = 3f;
    [SerializeField]
    private float flyForce = 750f;

    [Header("jump settings:")]
    [SerializeField]
    private float jointJump = 20f;
    [SerializeField]
    private float jointForceMax = 1000f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;
	//PlayerShootMethods
	void Start ()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No cam referenced");
            this.enabled = false;
        }
        Pistol.initializePistol(weapon);
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
	//PlayerSetupMethods
	void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //get rid of local player gui
            SetRecursiveLayer(playerGraphics, LayerMask.NameToLayer(dontDrawLayer));
            //create playerUI
            playerUIInstance = Instantiate(playerUI);
            playerUIInstance.name = playerUI.name;
            GetComponent<Player>().Setup();
        }



    }

    void SetRecursiveLayer(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            //goes through each child layer down heirarchy
            SetRecursiveLayer(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }


    void AssignRemoteLayer ()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents ()
    {
        for (int i = 0; i < disabledComponents.Length; i++)
        {
            disabledComponents[i].enabled = false;
        }
    }


    void OnDisable()
    {

        Destroy(playerUIInstance);
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
	//PlayerMotorMethods
	void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _camRotationX)
    {
        camRotationX = _camRotationX;
    }
    //gets vector for flying
    public void UseFly (Vector3 _flyForce)
    {
        flyForce = _flyForce;
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        
    }

    void PerformMovement ()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (flyForce != Vector3.zero)
        {
            rb.AddForce(flyForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            //Set rotation and clamp so cannot rotate upside down
            currentCamRotationX -= camRotationX;
            currentCamRotationX = Mathf.Clamp(currentCamRotationX, -cameraRotationLim, cameraRotationLim);

            //apply
            cam.transform.localEulerAngles = new Vector3(currentCamRotationX, 0f, 0f);
        }
    }


}
	//PlayerControllerMethods
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointJump);

    }

    private void Update()
    {
        if (PauseMenu.IsOn)
        {
            return;
        }

        //3d movement vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        Vector3 _velocity = (movHorizontal + movVertical).normalized * speed;

        motor.Move(_velocity);

        //rotation calc to turn around
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSens;

        motor.Rotate(_rotation);


        float xRot = Input.GetAxisRaw("Mouse Y");

        float _camRotationX = xRot * lookSens;

        motor.RotateCamera(_camRotationX);

        Vector3 _flyForce = Vector3.zero;

        //jump force
        if (Input.GetButton("Jump"))
        {
            _flyForce = Vector3.up * flyForce;
            SetJointSettings(0f);
        }else
        {
            SetJointSettings(jointJump);
        }

        motor.UseFly(_flyForce);
    }

    private void SetJointSettings (float _jointSpring)
    {
        joint.yDrive = new JointDrive {positionSpring = jointJump, maximumForce = jointForceMax };
    }
}
