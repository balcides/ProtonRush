using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Spawner.cs
/// 
/// Should also be known as EnemySpawner
/// The idea is to have a prefab and spawn point offset for this. Keeping it simple
/// 
/// 
/// </summary>

public class Spawner : MonoBehaviour {

    //spawn time
    [Header("Spawn Time")]
    public int spawnCooldownMin;
    public int spawnCooldownMax;
    [SerializeField] float countdown;
    float countStart;
    public bool randomCooldown;
    public float spawnrateExp;
    public int spawnrateExpBase;

    [Header("Misc")]
    //asset
    public Transform spawnAsset;

    //stats
    public Vector3 spawnPointOffset;

    //scripts
    GameMechanics GMX;
	GameManager GM;
	AssetManager AM;

	void Awake(){

		GMX = GameObject.Find("GameManager").GetComponent<GameMechanics>();
		GM = GameObject.Find("GameManager").GetComponent<GameManager> ();
		AM = GameObject.Find("AssetManager").GetComponent<AssetManager> ();
	}

	// Use this for initialization
	void Start () {

		spawnCooldownMin = 1;
		spawnCooldownMax = 17;

		countStart = Random.Range(spawnCooldownMin, spawnCooldownMax);
		countdown = countStart;

        spawnrateExp = 0.37f;
        spawnrateExpBase = spawnCooldownMax; //based on spreadsheet exp formula

	}
	
	// Update is called once per frame
	void Update () {
        Spawn();
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

			//reset count(old ramp system)
			//int killCount = GM.playerKillCount;
            //if (killCount >= 10) { spawnCooldownMax = 17; }
            //if (killCount >= 20) { spawnCooldownMax = 14; }
            //if (killCount >= 30) { spawnCooldownMax = 10; }
            //if (killCount >= 40) { spawnCooldownMax = 7; }
            //if (killCount >= 50) { spawnCooldownMax = 4; }
            //if (killCount >= 100) { spawnCooldownMax = 2; }

            //new ramp system
            spawnCooldownMax = (int) Mathf.Floor(GM.LevelupExpRate(GM.round,spawnrateExp,spawnrateExpBase) / GM.round);
            countdown = Random.Range(spawnCooldownMin, spawnCooldownMax);
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

		//set default spidercrab asset as level 1
		spawnAsset = AM.spidercrab[0];

		//check all player assets to see if they're above level 1
		GameObject[] cannons = GameObject.FindGameObjectsWithTag("Player");
		int highestLevel = 0;

		foreach (GameObject cannon in cannons) {
			int currentLevel = cannon.GetComponent<Unit> ().level;

			if (currentLevel > highestLevel) {
				highestLevel = currentLevel;
			}
		}
			
		//if so, random gen between level 1 and 2 assets
		if (highestLevel > 1) {
			//set spawn to that range item
			float randomGen = GMX.random;

			//spawn level2 spider if 75% over chance
			if (randomGen > 75) {
				spawnAsset = AM.spidercrab [1];
				spawnPoint = new Vector3 (transform.position.x,
					transform.position.y + (spawnPointOffset.y * 2),
					transform.position.z);
			}
		}
			
		// Create the Bullet from the Bullet Prefab
		var enemy = (GameObject)Instantiate (spawnAsset.gameObject, spawnPoint, transform.rotation);

		enemy.name = enemy.name + Random.Range (0, 100);

	}



}
