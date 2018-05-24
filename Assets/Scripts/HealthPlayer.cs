using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthPlayer : MonoBehaviour {

	private int _health; 			// Barre de vie du joueur
	public int _hpmax;
	private float _sweat; 			// Barre de sueur du joueur
	public int _swtmax;
	private Image _healthBar;		// Image de la barre de vie
	private Image _sweatBar; 		// Image de la barre de sueur
	private Rigidbody _rb; 			// Rigid Body du cycliste

	public float SWT_COEF;			// Paramètre pour régler la vitesse de remplissage de la sueur



    void Start () {
		
		_hpmax = 100;
		_swtmax = 100;
		_health = _hpmax;
		_sweat = 0;
		_rb = GameObject.Find("CyclistDos").GetComponent<Rigidbody>();
		_healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
		_sweatBar = GameObject.Find ("SweatBar").GetComponent<Image> ();

		// On commence avec une barre de sueur nulle
		_sweatBar.fillAmount = 0;

	}

	// Update is called once per frame
	void Update () {
		sweat ();
		updateSweat (); 

	}

	//===============================================================================================
	//							FONCTIONS POUR RETOURNER LES ATTRIBUTS
	//===============================================================================================

	public int getHealth(){
		// Retourne la vie actuelle du personnage
		return _health;
	}

	public int getHealthMax() {
		// Retourne la vie maximum du personnage
		return _hpmax;
	}

	public float getSweat(){
		// Retourne le niveau de sueur actuel
		return _sweat;
	}

	public int getSweatMax() {
		// Retourne le niveau maximal de sueur actuel
		return _swtmax;
	}


	//===============================================================================================
	//							MISE A JOUR GRAPHIQUE DE LA BARRE DE VIE / SUEUR
	//===============================================================================================

	public void UpdateHealth() {
		// Met à jour la barre de vie
		float amount = (float) _health / _hpmax;
		_healthBar.fillAmount = amount;
	}

	public void updateSweat() {
		// Met à jour la barre de sueur 
		float amount = (float) _sweat / _swtmax; 
		_sweatBar.fillAmount = amount; 
	}



	//===============================================================================================
	//							 	 GESTION DES DEGATS DU PERSONNAGE
	//===============================================================================================

	public void takeDamage(int amount){
		// Inflige des dégats
		_health -= amount;

		if (_health <0) {
			_health = 0;
			// Appeler une fonction pour dire qu'il crève !!!!
		}
		// On appelle la fonction pour mettre à jour la barre de vie
		Debug.Log("Damage Done");
		UpdateHealth ();
	}


	public void heal(int amount){
		// Soigne les dégâts
		_health += amount;

		if(_health > _hpmax){
			_health = _hpmax;
			// Appeler une fonction pour montrer que le joueur est full life
		}
		Debug.Log("Damage healed");
		UpdateHealth ();

	}

	//===============================================================================================
	//							 	 GESTION DE LA SUEUR DU PERSONNAGE
	//===============================================================================================

	public void sweat() {
		if (_sweat < _swtmax) {
			// Fonction pour remplir la barre de sueur
			// La barre de sueur se remplit proportionnellement à la vitesse du personnage
			Vector3 speed = _rb.velocity;
			_sweat += SWT_COEF * speed.sqrMagnitude;
		}
	}

}
