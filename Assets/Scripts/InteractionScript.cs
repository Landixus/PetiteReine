using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class handles the coroutines of every pick ups the cyclist can interact with.
 * The pick ups can also have scripts that does some additional things
 * */
public class InteractionScript : MonoBehaviour {

    public float bonusTime,malusTime;

    private Rigidbody m_rigidBody;
    private SpriteRenderer m_sprite;
    private HealthPlayer m_healthPlayer;
	private float m_collisionTime; 
	private bool m_hasCollided;

    private float timer=0f;
    private float time_to_blink = 0f;

	private float TIME_BTW_COLLISION = 1.0f;


	// -----------------------------------------------------------------------------------------
	// 							   START AND UPDATE FUNCTIONS 

    void Start () {
        m_rigidBody = GetComponent<Rigidbody>();
        m_sprite = GetComponentInChildren<SpriteRenderer>(); 								//Cyclist sprite
        m_healthPlayer = GameObject.Find("CyclistDos").GetComponent<HealthPlayer>(); 		// Vie du Cycliste
		m_collisionTime = 0f; 
		m_hasCollided = false;
	}

	void Update () {
		
		/*HandleCollision ();*/
        HandleBlinking();

	}



	//===============================================================================================
	//							 	 GESTION DES COLLISIONS
	//===============================================================================================


    private void OnTriggerEnter(Collider other)
    {

        /***************************************************************************************
		 * Cette fonction s'occupe de gérer les collisions avec les objets non personnages 
		 * 
		 * Typiquement elle appelle les fonctions relatives : 
		 * 		- Au clignotement du personnage 
		 * 		- A la gestion des collisions successives (temps minimal entre deux collisions
		 ***************************************************************************************/

        if (other.gameObject.CompareTag("Boost"))
        {
            Destroy(other.gameObject);
            StartCoroutine("SpeedUp");
        }
        else if(other.gameObject.CompareTag("NPC"))
        {
            m_hasCollided = true;
			Debug.Log ("Collision");
			m_healthPlayer.takeDamage(20);
            StartCoroutine("SpeedDown");
            //malus must only trigger once
            Destroy(other);
        }

    }


	/*private void HandleCollision() {
		// S'occupe de gérer le temps entre deux collisions

		if (m_hasCollided) {
			// S'il y a eu une collision, on ajoute du temps à la variable 
			m_collisionTime += Time.deltaTime;
		}
		if (m_collisionTime > TIME_BTW_COLLISION) {
			// Si on dépasse le temps minimal entre deux collisions, on reinitialise le temps 
			m_hasCollided = false; 
			m_collisionTime = 0f; 
		}
	}*/

	//===============================================================================================
	//							 	GESTION DES CLIGNOTEMENTS
	//===============================================================================================


    private void StartBlinking(float time)
    {
        time_to_blink = time;
    }

    private void HandleBlinking()
    {
        if (time_to_blink > 0)
        {
            timer += Time.deltaTime;
            time_to_blink -= Time.deltaTime;
            if (timer > 0.2f)
            {
                timer = 0f;
                m_sprite.enabled = !m_sprite.enabled;
            }
        }
        else if (!m_sprite.enabled)
        {
            m_sprite.enabled = true;
        }
    }



	//===============================================================================================
	//							 			COROUTINES
	//===============================================================================================


    IEnumerator SpeedUp()
    {
        float time = Time.time;
        GetComponent<CyclistMovement>().forwardForceMultiplier += 50;

        while (Time.time - time < bonusTime)
        {
            yield return null;
        }

        GetComponent<CyclistMovement>().forwardForceMultiplier -= 50;

    }

    IEnumerator SpeedDown()
    {
        
        StartBlinking(malusTime);
        
        float fm = GetComponent<CyclistMovement>().forwardForceMultiplier;

        float time = Time.time;
        float reduction = (fm > 50) ? -50 : -fm;

        GetComponent<CyclistMovement>().forwardForceMultiplier += reduction;

        float time2 = time;

        while (time2 - time < malusTime)
        {
            //progressively recover initial speed
            GetComponent<CyclistMovement>().forwardForceMultiplier -= (Time.time - time2)*(reduction/malusTime);
            time2 = Time.time;
            yield return null;
        }
        
    }



}
