using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marketMen : MonoBehaviour {


    private Animator m_animator;
	// Use this for initialization
	void Start () {
        m_animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animator.speed = 0;
            gameObject.layer = LayerMask.NameToLayer("Bike");
          
        }
    }

    
}
