﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Spawner.cs
/// 
/// Click on this tile and it shall spawn!
/// 
/// </summary>


public class SpawnTile : MonoBehaviour {

	//asset
	public Transform spawnAsset;

	//stats
	public Vector3 spawnPointOffset;
	public bool isSpawned = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){

		//on mouse click, spawn prefab at point offset if not spawned
		if (!isSpawned) {
			
			SpawnAsset ();
		}
	}

	//TODO: on cleanup, we can combine this with Spawner.cs version into a common class in GameMechanics.cs
	void SpawnAsset(){
	/*

		Spawn asset by instatiate

	 */
		//get transform and add offset
		Vector3 spawnPoint = new Vector3 (transform.position.x,
			transform.position.y + spawnPointOffset.y,
			transform.position.z);

		// Create the Bullet from the Bullet Prefab
		var spawned = (GameObject)Instantiate (spawnAsset.gameObject, spawnPoint, transform.rotation);

		//random num to name for ID
		spawned.name = spawned.name + Random.Range (0, 100);

		//assign this as cannon's tile spawner
		spawned.GetComponent<Cannon> ().tileSpawer = transform;

		//enabling this prevents further spawning on tile click
		isSpawned = true;

	}

}
