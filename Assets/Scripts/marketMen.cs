using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marketMen : MonoBehaviour {


    private Animator m_animator;
    private Rigidbody m_rigidBody;
    private AudioSource source;
    public AudioClip hurtSound1,hurtsound2;
  

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        
    }
    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animator.speed = 0;
            source.PlayOneShot((Random.Range(0, 1) > 0.5)?hurtSound1:hurtsound2);
            m_rigidBody.useGravity = true;
        }
    }
    

}
