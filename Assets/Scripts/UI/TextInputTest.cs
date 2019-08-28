using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInputTest : MonoBehaviour {

	public Transform playerScoreNameInput;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	public void PrintPlayerInput(){
	/*
		
		Print player input test

	*/

		print (playerScoreNameInput.GetComponent<InputField> ().text);
	}
}
