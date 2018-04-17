using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    private Vector3 offset;
    public GameObject player;
    public float min;
    private Vector3 previousRotation;
	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
        previousRotation = player.transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + offset;
        
	}
}
