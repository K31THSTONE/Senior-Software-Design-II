using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : NetworkBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSens = 3f;
    
    private PlayerMotor motor;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();

    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
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

        Vector3 _camRotation = new Vector3(xRot, 0f, 0f) * lookSens;

        motor.RotateCamera(_camRotation);



    }

    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}

