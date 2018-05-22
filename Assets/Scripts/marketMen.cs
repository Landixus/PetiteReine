using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marketMen : MonoBehaviour {


    private Animator m_animator;
    private Rigidbody m_rigidBody;
	// Use this for initialization
	void Start () {
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        
	}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animator.speed = 0;
            m_rigidBody.useGravity = false;
            gameObject.layer = LayerMask.NameToLayer("Bike");

            // Me :*hit* MarketMen: *fly away*
            m_rigidBody.AddForce(new Vector3(0,100,0));
            m_rigidBody.AddTorque(new Vector3(0, 100, 0));


        }
    }

    
}
