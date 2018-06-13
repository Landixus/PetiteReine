using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public GameObject player;

    public GameObject front, back;
    private Vector3 cyclistZ_axis;
    public Vector3 targetOffset, offset;
    public float originalDistance;

    public float multiplier;

    private float distance;
    private Vector3 eyePosition,eyeTarget,pointToLook;
    private Vector3 interpolatedTarget;
	// Use this for initialization
	public void Start () {
        distance = originalDistance;
        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;
	}
	
	// Update is called once per frame
	public void Update () {

        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;

        pointToLook = player.transform.position + offset;

        UpdateTargets();
        UpdateDistance();

        transform.position = interpolatedTarget;
        transform.LookAt(pointToLook);
        
    }

    private void UpdateTargets()
    {
        eyePosition = transform.position;
        eyeTarget = player.transform.position - cyclistZ_axis * distance + targetOffset;

        interpolatedTarget = eyePosition + (eyeTarget - eyePosition) * multiplier * Time.deltaTime;
                
    }

    private void UpdateDistance()
    {
        RaycastHit hit;
     
        int layer = 1 << LayerMask.NameToLayer("Bike");
        Vector3 origin = player.transform.position + offset;
        Vector3 direction = transform.position-origin;
        //Debug.DrawRay(b, a.normalized* a.magnitude, Color.white, 0.1f, false);
        if (Physics.Raycast(origin, direction.normalized, out hit, direction.magnitude*1.1f, ~layer))
        {
            
            Vector3 forward = direction.normalized * (hit.distance)*0.7f;
            
            Debug.DrawRay(origin, forward, Color.green);
            //interpolatedTarget = origin+forward;
            distance = hit.distance*0.7f ;
            
        }
        else
        {
            distance = distance + (originalDistance - distance) * multiplier * Time.deltaTime; 
        }
        Debug.Log(distance);
    }
}
