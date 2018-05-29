using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  This class handles the speed, steering and animation of the bike
 **/
public class CyclistMovement : MonoBehaviour
{

    public WheelCollider m_frontWheelCollider, m_backWheelCollider;

    public float forwardForceMultiplier;     //force multiplier for motorTorque 
    public float maxSteering;                //maximum angle for the front wheel rotation 
    public float brakeForce;
    public float maxSpeed;                   //maxSpeed is used for the animation handling 


    private Rigidbody m_rigidBody;
    private Animator m_animator;
   
    private float forward = 0f;
    private float sideways = 0f;

    private AntScript AntProxy;

	public float getForward() {
		return forward;
	}

    void Start()
    {

        m_rigidBody = GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
        AntProxy = GetComponent<AntScript>();
        AntProxy.Activate();
    }

    void FixedUpdate()
    {

        if (!AntProxy.isOn)
        {
            SetForward(Input.GetAxis("Vertical"));
        }
        sideways    = Input.GetAxis("Horizontal");

        HandleSteering();
        HandleAcceleration();
        
        HandleAnimationSpeed();
        
    }

    void HandleAcceleration()
    {
        if (forward >= 0)
        {
            m_backWheelCollider.motorTorque = forward * forwardForceMultiplier;
            m_frontWheelCollider.brakeTorque = 0;
            m_backWheelCollider.brakeTorque = 0;
        }
        else
        {
            m_backWheelCollider.brakeTorque = -forward * brakeForce;
            m_frontWheelCollider.brakeTorque = -forward * brakeForce;
        }
    }

    void HandleSteering()
    {
        m_frontWheelCollider.steerAngle = sideways * maxSteering;
    }

    void HandleAnimationSpeed()
    {
       
        m_animator.speed = (forward > 0) ? forward*maxSpeed : 0;

    }

    public void SetForward(float value)
    {
        forward = value;
    }
}
