using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Cannon.cs
/// 
/// Attach this to Command Center to handle this unit's rules and behavior
/// 
/// </summary>


public class CommandCenter : MonoBehaviour {

	//scripts
	GameMechanics GMX;
	Unit unit;


	void Awake(){

		GMX= Camera.main.GetComponent<GameMechanics> ();
		unit = GetComponent<Unit> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col){
	/*

		Handle collision events

	*/

		//if enemy runs into the player, take damage
		if (col.gameObject.tag == "Enemy") {

			Debug.Log ("command center hit by enemy");

			//deal damage to player asset based on enemy self attack score
			unit.hp -= col.gameObject.GetComponent<Unit>().attack;
		}

		//if we run out of hit points, die or destroy self
		//TODO: do we centralize this in GMX?
		if (unit.hp <= 0) {

			//once the command center is gone, it's game over
			GMX.GameOver();

			//self-destroy cannon
			GMX.SelfDestruct (gameObject);

		}
	}
}
