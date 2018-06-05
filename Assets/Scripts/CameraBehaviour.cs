using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public GameObject player;

    public GameObject front, back;
    private Vector3 cyclistZ_axis;
    public Vector3 originalOffset, targetOffset;
    public float originalDistance;

    public float multiplier;

    private float distance;
    private Vector3 offset;
    private Vector3 eyePosition,eyeTarget,pointToLook;
	// Use this for initialization
	void Start () {
        distance = originalDistance;
        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {

        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;

        pointToLook = player.transform.position + targetOffset;
        
        UpdateEye();
        UpdateDistance();
        transform.LookAt(pointToLook);
        Debug.DrawLine(pointToLook, transform.position);
    }

    private void UpdateEye()
    {
        eyePosition = transform.position;
        eyeTarget = player.transform.position - cyclistZ_axis * distance + offset;

        transform.position = eyePosition + (eyeTarget - eyePosition) * multiplier * Time.deltaTime;
                
    }

    private void UpdateDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, eyePosition-player.transform.position, out hit, (eyePosition - player.transform.position).magnitude, LayerMask.NameToLayer("Default")))
        {
            distance = Vector3.Project( hit.point-player.transform.position, cyclistZ_axis).magnitude;
            //offset = offset * distance / originalDistance;
            Debug.Log(distance);
            
        }
        else
        {
            distance = originalDistance;
            offset = originalOffset;
        }
    }
}
