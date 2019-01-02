using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Enemy.cs
/// 
/// Not sure if needed, used to handle enemy damage on collision and any other type handling for common enemy units.
/// 
/// Note: if the same can be used for player, transfer or consolidate to Unit.cs
/// 
/// </summary>


public class Enemy : MonoBehaviour {

	//stats
	public Vector3 direction;

	//mechanics
	public float selfDestructCountdown;

	//scripts
	//AssetManager AM;
	GameMechanics GMX;
	GameManager GM;

	void Awake(){

		//AM = Camera.main.GetComponent<AssetManager> ();
		GMX= Camera.main.GetComponent<GameMechanics> ();
		GM= Camera.main.GetComponent<GameManager> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//move unit on update
		Move ();
		TimeDestruct ();
		
	}

	void OnCollisionEnter (Collision col)
	{
		
		if(col.gameObject.tag == "Photon")
		{
			//destroy photon on collision enter against enemy
			Destroy(col.gameObject);

			//reduce enemy's hp by bullet damage
			//example: cannon's stat sets photon damage number
			//TODO: see if this can be replace with Unit.instance.hp. Same with Photon.instance.damage
			GetComponent<Unit>().hp -= col.gameObject.GetComponent<Photon>().damage;

		}

		//if we run out of hit points, die or destroy self
		if (GetComponent<Unit>().hp <= 0) {

			//Award player XP for kill
			GM.playerXP += GetComponent<Unit>().xp;
			GM.playerXPscore += GetComponent<Unit>().xp;
			GM.playerKillCount += 1;
			
			//SelfDestruct ();
			GMX.SelfDestruct (gameObject);

		}

		//if enemy runs into the player, destroy self
		if (col.gameObject.tag == "Player") {

			//Award player XP for kill
			GM.playerXP += GetComponent<Unit>().xp;
			GM.playerXPscore += GetComponent<Unit>().xp;

			//SelfDestruct();
			GMX.SelfDestruct (gameObject);

			//deal damage to player asset based on enemy self attack score
			col.gameObject.GetComponent<Unit>().hp -= GetComponent<Unit>().attack;
		}
	}

	void Move(){
	/*

		Auto moves the enemy asset

	*/
		//asset moves in direction set by speed set
		//move this in direction by deltaTime
		transform.Translate(direction * Time.deltaTime * GetComponent<Unit>().speed);
	}

	void TimeDestruct(){

		selfDestructCountdown = selfDestructCountdown - Time.deltaTime;

		if (selfDestructCountdown<= 0) {
			GMX.SelfDestruct (gameObject);
		}
	}


		
}
