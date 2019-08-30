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
    public float zLimit;       //bounds by which photons self destruct

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

		// Destroy the bullet after num seconds if not past bounds of z
        if (gameObject.transform.position.z > zLimit) Destroy(gameObject);
        else Destroy(gameObject,longevity);

    }
}
