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
	public Vector3 leftBoundary; 
	public Vector3 rightBoundary;
	public int nbStep;

	// Private Variables
	private bool _forward; 
	private int _currentStep;
	private bool _collided;
	private Rigidbody _rigidbody;


	// Use this for initialization
	void Start () {
		_currentStep = 0;
		_forward = true; 
		_collided = false;
		_rigidbody = GetComponent<Rigidbody> ();
		_rigidbody.useGravity = false;

		
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Cyclist")
		{
			_collided = true;
			_rigidbody.useGravity = true;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (!_collided) {
			// Si le personnage a effectué le nombre de pas idéal, on le réinitialise
			// et on change la direction 
			if (_currentStep == nbStep) {
				_forward = !_forward;
				_currentStep = 0;
			}

			// Si le personnage va dans un sens, on ajoute le vecteur direction correspondant
			Vector3 direction = rightBoundary - leftBoundary; 
			if (_forward) {
				transform.Translate (direction * 1 / nbStep); 
				_currentStep++;
			} else {
				transform.Translate (-direction * 1 / nbStep);
				_currentStep++;
			}
		}

	}
}
