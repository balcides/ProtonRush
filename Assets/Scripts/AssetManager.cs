﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// assetManager.cs
/// 
/// Store global needed assets for game
/// 
/// </summary>

public class AssetManager : MonoBehaviour {

	//Gameplay
	public Transform[] cannons = new Transform[2];
	public Transform[] spidercrab = new Transform[2];

	//FX
	public Transform explosion;


	//Assign vars before start
	void Active(){



	}

	// Use this for initialization
	void Start () {

        //disable score text on start
        //playerScoreName.gameObject.GetComponent<Text> ().enabled = false;
        //playerScoreKills.gameObject.GetComponent<Text> ().enabled = false;
        //playerScoreXpScore.gameObject.GetComponent<Text> ().enabled = false;
        //playerScoreBKG.gameObject.SetActive(false);
        //playerScoreSubmitBtn.gameObject.SetActive(false);
        //playerScoreNameInput.gameObject.SetActive(false);
        //tryAgainBtn.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
