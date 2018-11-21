using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// GameMechanics.cs
/// 
/// Manages common elements of gameplay to keep Game Manager about rules and consequences
/// while mechanics deals with common things like explosion on destroy for all units and things like that. Common game elements per units
/// 
/// note: you may want to attach this to camera alongside the GameManager.cs
/// 
/// </summary>

public class GameMechanics : MonoBehaviour {

	//scripts
	AssetManager AM;

	void Awake(){

		AM = GetComponent<AssetManager> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void SelfDestruct(GameObject asset){
	/*

		Asset destroys and all things thereof

	*/

		//release explosion effect
		Instantiate (AM.explosion.gameObject, asset.transform.position, asset.transform.rotation);

		//destroy self 
		Destroy (asset.gameObject);


	}

	//Game over
	public void GameOver(){

		//Print game over message
		AM.gameStatus.GetComponent<Text>().text = "GAME OVER";

		//stop all bugs (or destroy)
		//TODO: maybe make this a global command (allEnemies() or something)
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies){ SelfDestruct (enemy); }

		//disable all the spawners
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
		GameObject[] spawnTiles = GameObject.FindGameObjectsWithTag("SpawnTile");
		GameObject[] cannons = GameObject.FindGameObjectsWithTag("Player");

		foreach (GameObject spawner in spawners) {		spawner.GetComponent<Spawner> ().enabled = false;	}		
		foreach (GameObject spawnTile in spawnTiles) {	spawnTile.GetComponent<SpawnTile> ().isSpawned = true; }
		foreach (GameObject cannon in cannons) {  		cannon.GetComponent<Cannon> ().enabled = false; }
	}

}
