using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Photon.cs
/// 
/// Attach this to bullet or photon. Meant to handle having it move in one direction
/// 
/// </summary>


public class Photon : MonoBehaviour {

	//Stats
	public int damage;
	public float longevity;
	public float speed;
	public Vector3 direction;

	//initialize
	public static Photon instance = null;

	void Awake(){

		//works best when you need to run a method or run a class but not for update info
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		//move this in direction by deltaTime
		transform.Translate(direction * Time.deltaTime * speed);


		// Destroy the bullet after 2 seconds
		Destroy(gameObject, longevity);
		
	}
}
