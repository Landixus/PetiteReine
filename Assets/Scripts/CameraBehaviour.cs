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
    private Vector3 interpolatedTarget;
	// Use this for initialization
	public void Start () {
        distance = originalDistance;
        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;
	}
	
	// Update is called once per frame
	public void Update () {

        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;

        pointToLook = player.transform.position + targetOffset;

        UpdateTargets();
        UpdateDistance();

        transform.position = interpolatedTarget;
        transform.LookAt(pointToLook);
        Debug.DrawLine(pointToLook, transform.position);
    }

    private void UpdateTargets()
    {
        eyePosition = transform.position;
        eyeTarget = player.transform.position - cyclistZ_axis * distance + offset;

       interpolatedTarget = eyePosition + (eyeTarget - eyePosition) * multiplier * Time.deltaTime;
                
    }

    private void UpdateDistance()
    {
        RaycastHit hit;
        Debug.Log(LayerMask.NameToLayer("Bike"));
        int layer = 1 << LayerMask.NameToLayer("Bike");
        if (Physics.Raycast(player.transform.position, (interpolatedTarget-player.transform.position).normalized, out hit, (interpolatedTarget - player.transform.position).magnitude, ~layer))
        {
            distance = Vector3.Project( hit.point-player.transform.position, cyclistZ_axis).magnitude;
            //offset = offset * distance / originalDistance;
            Debug.Log(distance);
            Debug.Log(hit.collider.gameObject.tag);
            Debug.Log(hit.collider.gameObject);
        }
        else
        {
            distance = originalDistance;
            offset = originalOffset;
        }
    }
}
