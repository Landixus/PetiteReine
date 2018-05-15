using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public GameObject player;

    public GameObject front, back;
    private Vector3 cyclistZ_axis;
    public Vector3 offset, targetOffset;
    public float distance;

    public float multiplier;

    private Vector3 eyePosition,eyeTarget,pointToLook;
	// Use this for initialization
	void Start () {

        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {

        cyclistZ_axis = (front.transform.position - back.transform.position).normalized;

        pointToLook = player.transform.position + targetOffset;
        
        UpdateEye();

        transform.LookAt(pointToLook);
	}

    private void UpdateEye()
    {
        eyePosition = transform.position;
        eyeTarget = player.transform.position - cyclistZ_axis * distance + offset;

        transform.position = eyePosition + (eyeTarget - eyePosition) * multiplier * Time.deltaTime;
                
    }
}
