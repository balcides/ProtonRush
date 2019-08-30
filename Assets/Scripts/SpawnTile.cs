using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Spawner.cs
/// 
/// Click on this tile and it shall spawn!
/// (Should also be known as player spawner)
/// 
/// </summary>


public class SpawnTile : MonoBehaviour {

	//asset
	public Transform spawnAsset;

	//stats
	public Vector3 spawnPointOffset;
	public bool isSpawned = false;

	//script
	GameManager GM;
	GameMechanics GMX;
    GuiManager GUIM;
    AssetManager AM;

    //color schemes
    Material mat;
    private Color defaultColor;
    private Color highlightColor;


	void Awake(){
		GM = GameObject.Find("GameManager").GetComponent<GameManager> ();
		GMX = GameObject.Find("GameManager").GetComponent<GameMechanics> ();
        GUIM = GameObject.Find("GUIManager").GetComponent<GuiManager>();

        //get current tile color and set it to detault
        mat = GetComponent<Renderer>().material;
        defaultColor = new Color(1f,1f,1,0f);
        highlightColor = Color.white;
        mat.color = defaultColor;
    }


	// Use this for initialization
	void Start () {
        AM = GameObject.Find("AssetManager").GetComponent<AssetManager>();
    }


	// Update is called once per frame
	void Update () {
		
	}


    //TODO: on cleanup, we can combine this with Spawner.cs version into a common class in GameMechanics.cs
    public void SpawnAsset(){
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
		//TODO: use random generated from camera
		spawned.name = spawned.name + Random.Range (0, 100);

		//assign this as cannon's tile spawner
		spawned.GetComponent<Cannon> ().tileSpawer = transform;

		//enabling this prevents further spawning on tile click
		isSpawned = true;

	}


    private void GetCreditXPDiff(out int unitXP,out int xpDiff) {
        unitXP = spawnAsset.GetComponent<Unit>().cryptoXP;
        xpDiff = GMX.CreditsRemaining(unitXP);
    }


    void OnMouseDown() {

        //on mouse click, spawn prefab at point offset if not spawned
        if(!isSpawned) {
            int cryptoXP, xpDiff;
            GetCreditXPDiff(out cryptoXP,out xpDiff);

            //if there's enough XP, spawn, else nothing. Note: we're using playerXP as a cost per unit
            if(-1 * xpDiff >= 0) {

                SpawnAsset();
                //isSpawned = true; //set independent of
                GM.crypto -= cryptoXP;

            } else {
                Debug.Log("Not enough XP to purchase cannon. Must have " + xpDiff + " credits to purchase");
                GUIM.gameInfo.text = "Not enough XP to purchase cannon. Must have " + xpDiff + " credits to purchase";
            }
        }
    }


    private void OnMouseOver() {
        int cryptoXP, xpDiff;
        GetCreditXPDiff(out cryptoXP,out xpDiff);
        GUIM.gameInfo.text = "Place Cannon: " + cryptoXP + " Crypto";


        //highlight tile
        mat.color = highlightColor;
    }

    private void OnMouseExit() {

        //restore tile matieral
        mat.color = defaultColor;
    }
}
