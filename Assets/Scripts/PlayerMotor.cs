using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

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
