using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSens = 3f;
    [SerializeField]
    private float flyForce = 750f;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();

    }

    private void Update()
    {
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

        Vector3 _flyForce = Vector3.zero;

        //jump force
        if (Input.GetButton("Jump"))
        {
            _flyForce = Vector3.up * flyForce;
        }

        motor.UseFly(_flyForce);
    }
}
