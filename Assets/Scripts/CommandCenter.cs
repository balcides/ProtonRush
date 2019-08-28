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
	GameManager GM;
	Unit unit;
    GuiManager GUIM;

	void Awake(){

		GMX= Camera.main.GetComponent<GameMechanics> ();
		GM= Camera.main.GetComponent<GameManager> ();
		unit = GetComponent<Unit> ();
        GUIM = GameObject.Find("GUIManager").GetComponent<GuiManager>();

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

	void OnMouseDown(){

		//when clicking on the command center, charge 500 xp for restoring 50% of it's HP

		//look at num of credits
		int numOfCredits = GM.crypto;

		//if there's enough, convert to level 2
		if (numOfCredits >= GM.creditCostRepairCmdCenter) {

			//reduce credits 
			GM.crypto -= GM.creditCostRepairCmdCenter;

			//heal command center 50% of total
			int healPoints = Mathf.RoundToInt(unit.maxHp * 0.5f);
			unit.hp = unit.hp + healPoints;
		
		} else {

			print ("Not enough XP to repair command center. Must have " + (GMX.creditsRemaining (GM.creditCostRepairCmdCenter)) + " credits to restore");
            GUIM.gameStatusText.text = "Not enough XP to repair command center. Must have " + (GMX.creditsRemaining(GM.creditCostRepairCmdCenter)) + " credits to restore";
        }

	}
}
