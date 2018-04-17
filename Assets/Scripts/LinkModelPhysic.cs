using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkModelPhysic : MonoBehaviour {

    public WheelCollider frontWheelCollider, backWheelCollider;
    public Transform frontWheelModel, backWheelModel;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 position;
        Quaternion rotation;

        frontWheelCollider.GetWorldPose(out position, out rotation);

        frontWheelModel.position = position;
        frontWheelModel.rotation = rotation;
        
        backWheelCollider.GetWorldPose(out position, out rotation);

        backWheelModel.position = position;
        backWheelModel.rotation = rotation;

        
    }
}
