using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	//FX
	public Transform explosion;

	//HUD
	public Transform xpCounter;
	public Transform killCounter;
	public Transform gameStatus;

	//Assign vars before start
	void Active(){

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
