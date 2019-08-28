using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// Hud.cs
/// 
/// Manages health bar and any other display elements in 3D. For game menu, try MenuManager.cs
/// It auto updates based on unit's hp
/// 
/// </summary>


public class Hud : MonoBehaviour
{
	//Assets
	public Transform healthBar;
	public Transform healthBarBKG;

	//stats
	int maxHP;
	float maxLength;
	Vector3 healthBarStartPos;

	// Use this for initialization
	void Start (){

		//get full health num of unit
		maxHP = GetComponent<Unit>().hp;

		//get full health num of unit, size is 100%, get scale
		maxLength = healthBar.localScale.x;

		//used to help start the left align of healthbar
		healthBarStartPos = healthBar.position;

	}
	
	// Update is called once per frame
	void Update (){
		
		AdjustHealthBar ();
	}

	void AdjustHealthBar(){
	/*

	 	Adjust size of health bar at runtime
	 	
	 */

		//check health
		int currentHealth = GetComponent<Unit>().hp;

		//get percent of health from total
		float currentPercentHealth = (currentHealth*100)/maxHP;
        print(transform.name + " " + currentPercentHealth);

		//scale along x to % of health
		float currentLengthX = (maxLength * currentPercentHealth)/100;

		//set healthbar size to new percentage size
		healthBar.localScale = new Vector3(currentLengthX, healthBar.localScale.y, healthBar.localScale.z);

		//update healthbar pos
		float scalePercent = (currentLengthX * 100)/maxLength;

		//get x but if y enabled, use that (really z)
		float healthStartPos = healthBarStartPos.x;
		healthStartPos = healthBarBKG.position.x;

		//get left alignment value
		float leftAlign = (0.5f *healthStartPos) + ((0.5f * healthStartPos* scalePercent) / 100);

		//healthbar updates to new pos
		//Debug.Log("healthbarBKG pos Z = " + healthBarBKG.position.z);
		healthBar.position = new Vector3(healthBar.position.x, healthBar.position.y, healthBar.position.z);
		//healthBar.position = new Vector3(leftAlign, healthBar.position.y, healthBar.position.z);
	}
		
}

