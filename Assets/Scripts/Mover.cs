using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float rotationSpeed;
    public float speed;
    public GameObject front, back, rotationMark;
	// Update is called once per frame
	void FixedUpdate () {

     
        float dv = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");
   

        Vector3 translationAxis = front.transform.position - back.transform.position;
        Vector3 rotationAxis = rotationMark.transform.position - front.transform.position;
        print(Vector3.Normalize(rotationAxis) * rotate * rotationSpeed);
        GetComponent<Rigidbody>().AddForce(Vector3.Normalize(translationAxis) * dv * speed);
        GetComponent<Rigidbody>().AddTorque(Vector3.Normalize(rotationAxis)*rotate*rotationSpeed);
        
        


    }
}
