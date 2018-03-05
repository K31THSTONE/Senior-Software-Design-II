using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSens = 3f;
    [SerializeField]
    private float flyForce = 750f;

    [Header("jump settings:")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointJump = 20f;
    [SerializeField]
    private float jointForceMax = 1000f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointJump);

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
        joint.yDrive = new JointDrive { mode = jointMode, positionSpring = jointJump, maximumForce = jointForceMax };
    }
}
