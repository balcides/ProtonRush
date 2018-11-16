using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Spawner.cs
/// 
/// The idea is to have a prefab and spawn point offset for this. Keeping it simple
/// 
/// </summary>

public class Spawner : MonoBehaviour {

	//asset
	public Transform spawnAsset;

	//stats
	public Vector3 spawnPointOffset;

	//spawn time
	public int spawnCooldown;
	float countdown;
	float countStart;

	// Use this for initialization
	void Start () {

		countStart = spawnCooldown;
		countdown = countStart;
		
	}
	
	// Update is called once per frame
	void Update () {
		Spawn ();
	}

	void Spawn(){
	/*

		Spawn asset after countdown runs to the end

	 */

		countdown = countdown - Time.deltaTime;

		//every num of seconds
		if (countdown <= 0) {

			//spawn asset from target position
			SpawnAsset();

			//reset count
			countdown = countStart;
		}
	}

	void SpawnAsset(){
	/*

		Spawn asset by instatiate

	 */
		//get transform and add offset
		Vector3 spawnPoint = new Vector3 (transform.position.x,
			                     		  transform.position.y + spawnPointOffset.y,
		                    		  	  transform.position.z);

		// Create the Bullet from the Bullet Prefab
		var enemy = (GameObject)Instantiate (spawnAsset.gameObject, spawnPoint, transform.rotation);

		enemy.name = enemy.name + Random.Range (0, 100);

	}



}
