using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private bool _flag; 
	private float _time; 
	private int _minutes; 
	private int _seconds; 
	private Text _uiDisplay;

	// Use this for initialization
	void Start () {
		_flag = true; 
		_time = 0; 
		_minutes = 0; 
		_seconds = 0; 
		_uiDisplay = GameObject.Find ("Clock").GetComponent<Text>();
	}

	void initTimer() {
		_flag = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (_flag) {
			_time += Time.deltaTime; 
			_minutes = Mathf.FloorToInt (_time / 60);
			_seconds = Mathf.FloorToInt (_time - _minutes * 60); 
			_uiDisplay.text = string.Format ("{0} : {1}", _minutes, _seconds);

		}
	}
}
