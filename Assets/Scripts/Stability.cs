using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 *  Script that controls the stability of the bicycle it proposes 3 stabilizations options.
 *  The first one is a minimisation of the error along a stability axis, it uses PID regulator
 *  The second one is a vertical stabilization, it puts a vertical force to make the bicycle stick on the ground
 *  The third one is a simple freeze of the rotation along the z-axis
 *  
 *  The three methods are turned off and on by three booleans : pid,vertical_stabilization and simple_freeze
 *  
 *  The scripts also requires 6 transforms to create a local referential of the bicycle 
 *  Finally, the script requires a center of mass, 
 *  if it is not given the default center of mass of the bicycle will be used
 **/
public class Stability : MonoBehaviour {

    //Public attributes
    public Transform com; //center of mass

    public Transform front, up, down, back, left, right;//used to build the local referential
                                                        //up is also used to apply the PID correction force
                                                        //front is also used to apply the vertical stabilisation force

    public bool simple_freeze;

    public bool pid;
    public float pid_multiplier;    //a multiplier for the correction
    public float max_tilt;          //the maximum tilt when the player is turning (from 0 to 90)
    public float Ki, Kp, Kd, bias;  //the coefficients of the PID regulator

    public bool vertical_stabilization;
    public float vertical_multiplier;//a multiplier for the down force applied 

    //Private attributes
    private Rigidbody m_rigidBody; 

    private Vector3 world_Y_axis,m_Z_axis,m_X_axis,m_Y_Axis,m_stability_Axis;//the local referential

    private float forward_acceleration,side_acceleration,slope;

    //attributes used to compute the output of the PID regulator ;
    //d_err = derivative term, i_err = integral term
    private float d_err, error;
    private Vector3 i_err,lastError,output;

    public WheelCollider front_wheel_collider,back_wheel_collider;
    
    void Start () {

        m_rigidBody = GetComponent<Rigidbody>();

        //changing the center of mass
        if (com != null)
        {
            m_rigidBody.centerOfMass = new Vector3(com.localPosition.x, com.localPosition.y, com.localPosition.z);
        }

        //not really useful but more explicit to me
        world_Y_axis = Vector3.up;


        i_err       = new Vector3(0, 0, 0);
        lastError   = new Vector3(0, 0, 0);
        output      = new Vector3(0, 0, 0);
        
    }

    void FixedUpdate() {

        //warning : all the global variables are updated here so they can be used in the next functions
        UpdateAttributes();
       
        if (vertical_stabilization) VerticalStabilization();

        if(pid) PIDregulator();

        if(simple_freeze) Freeze();

        //PS: the pid regulator is now really efficient so the two other functions are deprecated

    }
    private void UpdateAttributes()
    {

        UpdateLocalAxis();
   
        forward_acceleration    = Input.GetAxis("Vertical");
        side_acceleration       = Input.GetAxis("Horizontal");

        UpdateSlope();
    }

    /**
     * fonction that computes the output correction among a given dimension, the error is the difference bewtween
     * the stability_axis and the local Y axis, it means that the local Y axis will try to become the stability axis
     * */
    float PID(int dim)
    {
        
        switch (dim)
        {
            case 0:
                error = (m_stability_Axis - m_Y_Axis).x;
                break;
            case 1:
                error = (m_stability_Axis - m_Y_Axis).y;
                break;
            case 2:
                error = (m_stability_Axis - m_Y_Axis).z;
                break;
        }

       
        if (error < 180)
        {
            error = -error;
        }
        else
        {
            error = 360 - error;
        }

        d_err = (error - lastError[dim]) / Time.fixedDeltaTime;
        i_err[dim] += error * Time.fixedDeltaTime;

        lastError[dim] = error;

        return Kp * error + Ki * i_err[dim] + Kd * d_err + bias;

    }

    //computes the vertical stabilization force
    private void VerticalStabilization()
    {
        if (forward_acceleration > 0)
        {
            m_rigidBody.AddForceAtPosition(new Vector3(0, -forward_acceleration * vertical_multiplier, 0), front.position);
        }
    }

    //computes and applies the PID regulator output force
    private void PIDregulator()
    {            
        //check the acceleration to compute the stability axis
        if (side_acceleration != 0)
        {
            m_stability_Axis = (world_Y_axis + side_acceleration*(max_tilt/90) * m_X_axis).normalized;
        }
        else
        {
            m_stability_Axis = world_Y_axis;
        }

        //adjusting the stability axis to the slope
        m_stability_Axis = Quaternion.AngleAxis(slope*180/Mathf.PI, m_X_axis) * m_stability_Axis;

        output.x = PID(0);
        output.y = PID(1);
        output.z = PID(2);

        if (front_wheel_collider.isGrounded || back_wheel_collider.isGrounded)
        {
            m_rigidBody.AddForceAtPosition(-output * pid_multiplier, up.position);
        }

        Debug.DrawLine(up.position, up.position - output.normalized,Color.red);
        Debug.DrawLine(com.position, com.position + m_stability_Axis, Color.yellow);
    }

    //prevent the bike from rotating around the WORLD Z and X axis and not a local axis, 
    //this fonction causes a lot of bugs 
    private void Freeze()
    {
        m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ;
        m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX;
        m_rigidBody.transform.eulerAngles = new Vector3(0, m_rigidBody.transform.eulerAngles.y, 0);
        
    }

    //update the slope
    private void UpdateSlope()
    {
        float sina = m_Z_axis.y; //sin(a) ,a is the angle of the slope the bike is rolling on

        slope = (sina > 0) ? -Mathf.Asin(Mathf.Abs(sina)) : Mathf.Asin(Mathf.Abs(sina));
    }

    private void UpdateLocalAxis()
    {
        m_Z_axis = (front.position - back.position).normalized; //forward axis, the bike is always driving inthis direction
        m_X_axis = (right.position - left.position).normalized;
        m_Y_Axis = (up.position - down.position).normalized;
    }
}
