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

	//timers
	float leaderboardTimer;
	bool enableLeaderboardTimer = false;

	//scripts
	public float random;
	AssetManager AM;
	GameManager GM;

	void Awake(){

		AM = GetComponent<AssetManager> ();
		GM = GetComponent<GameManager> ();
	}


	// Use this for initialization
	void Start () {
		
		leaderboardTimer = 3f;
	}


	// Update is called once per frame
	void Update () {

		//global random num generator
		random = Random.Range (0, 100);

		//start leaderboard timer (called on game over)
		if (enableLeaderboardTimer) {
			leaderboardTimer -= 1 * Time.deltaTime;

			//load leaderboard when timer reaches 0
			if (leaderboardTimer <= 0) {
				leaderboardTimer = 0;
				GM.LoadLeaderboard ();
			}
		}
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

		//enable leaderboard
		enableLeaderboardTimer = true;
	}


	public float Randomizer(float min, float max, float multiplier){

		random = random * multiplier;
		if (random < min) {			random = min; 
		} else if (random > max) {	random = max;	}

		return random;
	}

	//TODO: on cleanup, we can combine this with Spawner.cs version into a common class in GameMechanics.cs
	public void SpawnAsset(Transform cannonAsset, Transform tileSpawner, Vector3 spawnPointOffset){
	/*

		Spawn asset by instatiate

		cannonAsset = Cannon to use (level 1, 2, etc)
		spawnPointOffset = position where cannon spawns from
		tileSpawner = assigns to spawed cannon to know which tile it's rooted from (probably not needed but added just in case)
		

	 */
		//get transform and add offset
		Vector3 spawnPoint = new Vector3 (tileSpawner.position.x,
			tileSpawner.position.y + spawnPointOffset.y,
			tileSpawner.position.z);

		// Create the Cannon from the Cannon Prefab
		var spawned = (GameObject)Instantiate (cannonAsset.gameObject, spawnPoint, tileSpawner.rotation);

		//random num to name for ID
		spawned.name = spawned.name + Random.Range (0, 100);

		//assign this as cannon's tile spawner
		spawned.GetComponent<Cannon> ().tileSpawer = tileSpawner;

	}

	public int creditsRemaining(int creditCost){
	/*

		Gets the remaining number of credits. Convenient for printouts

	*/
		int numOfCredits = GM.playerXP;
		int creditDiff = creditCost - numOfCredits;

		return creditDiff;
	}



}
