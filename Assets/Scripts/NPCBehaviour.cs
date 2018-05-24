using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour {
	/* 
	 *	Ce script permet de donner un mouvement au personnage auquel on applique le script
	 * 	Il se déplace entre les deux points leftBoundary et rightBoundary en faisant des 
	 * 	aller-retour
	*/

	// Public Variables
	public Transform leftBoundary; 
	public Transform rightBoundary;
	public int nbStep;
    public string type;

	// Private Variables
	private int _forward; 
	private int _currentStep;
	private bool _collided;
	private Rigidbody _rigidbody;
    private SpriteRenderer m_sprite;

	// Use this for initialization
	void Start () {
		_currentStep = 0;
		_forward = 1;
		_collided = false;
		_rigidbody = GetComponent<Rigidbody> ();
		_rigidbody.useGravity = false;
        m_sprite = GetComponent<SpriteRenderer>();
		
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.CompareTag("Player"))
		{
			_collided = true;
			_rigidbody.useGravity = true;
		}
	}

	
	//meme si c'est pas physique on a besoin d'un pas de temps constant
	void FixedUpdate () {
		if (!_collided) {
			// Si le personnage a effectué le nombre de pas idéal, on le réinitialise
			// et on change la direction 
			if (_currentStep == nbStep) {
				_forward = -_forward;

                Flip();
				_currentStep = 0;
			}

			// Si le personnage va dans un sens, on ajoute le vecteur direction correspondant
			Vector3 direction = rightBoundary.position - leftBoundary.position; 
			
			transform.Translate (direction * _forward / nbStep); 
			_currentStep++;
            
			
		}

	}

    public void Flip()
    {
        if (type == "mamie")
        {
            m_sprite.flipX = !m_sprite.flipX;
        }
        if (type == "hommeChien")
        {
            m_sprite.flipX = !m_sprite.flipX;
            BoxCollider[] m_colliders = GetComponents<BoxCollider>();
            
            foreach(BoxCollider c in m_colliders)
            {
                c.enabled = !c.enabled;
            }
          
        }

    }
}
