using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]

public class PlayerController : NetworkBehaviour {
	
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
    private Camera camMotor;


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
    private float flyForceController = 1000f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterRegenSpd = 0.4f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [Header("jump settings:")]
    [SerializeField]
    private float jointJump = 20f;
    [SerializeField]
    private float jointForceMax = 1000f;


    //private PlayerMotor motor;
    private ConfigurableJoint joint;

    //PlayerControllerMethods
    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointForceMax };
    }

    //PlayerShootMethods
    void Start ()
    {
		//PlayerSetupStart
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

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No playerUI component on PlayerUI prefab");

            }
            else
            {
                ui.SetController(GetComponent<PlayerController>());
            }
            GetComponent<Player>().Setup();
        }
		
		//PlayerShootStart
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No cam referenced");
            this.enabled = false;
        }
        weapon.initializePistol();
        weaponGraphics.layer = LayerMask.NameToLayer(weaponLayername);
		
		//PlayerMotorStart
		rb = GetComponent<Rigidbody>();
		
		//PlayerControllerStart
		//motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointJump);
    }

    void Update()
    {
		//PlayerShootUpdate
        if (PauseMenu.IsOn)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        //should correct physics when landing on objects around map or going over
        RaycastHit _hit;
        if (Physics.Raycast (transform.position, Vector3.down, out _hit, 100f))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }
		
		//PlayerControllerUpdate
		//3d movement vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        Vector3 _velocity = (movHorizontal + movVertical).normalized * speed;

        this.Move(_velocity);

        //rotation calc to turn around
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSens;

        this.Rotate(_rotation);


        float xRot = Input.GetAxisRaw("Mouse Y");

        float _camRotationX = xRot * lookSens;

        this.RotateCamera(_camRotationX);

        Vector3 _flyForce = Vector3.zero;

        //jump force
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
            //flying
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if (thrusterFuelAmount >= 0.01f)
            {
                _flyForce = Vector3.up * flyForceController;
                SetJointSettings(0f);
            }

        }else
        //not flying
        {
            thrusterFuelAmount += thrusterRegenSpd * Time.deltaTime;
            SetJointSettings(jointJump);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);

        this.UseFly(_flyForce);
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
