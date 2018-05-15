using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour {

	private int _health; // Barre de vie du joueur
	public int _hpmax;
	private int _sweat; // Barre de sueur du joueur
	private int _swtmax


	public void Start () {
		_hpmax = 100;
		_swtmax = 100;
		_health = _hpmax;
		_sweat = _swtmax;
	}

	int getHealth(){
		return _health;
	}
	int getSweat(){
		return _sweat;
	}

	public void takeDamage(int amount){
		_health -= amount;

		if (_health <0) {
			_health = 0;
			// Appeler une fonction pour dire qu'il crève !!!!
		}
	}


	public void heal(int amount){
		_health += amount;

		if(_health > _hpmax){
			_health = hpmax;
			// Appeler une fonction pour montrer que le joueur est full life
		}
	}

	public void sweat(int speed){
		_sweat -= 0.2*speed;
	}



	// Update is called once per frame
	void Update () {

	}
}
