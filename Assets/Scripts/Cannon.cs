using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[RequireComponent(typeof(Unit))]

/// <summary>
///
/// Cannon.cs
/// 
/// Attach this to anything and it will fire projectiles of whatever type
/// 
/// </summary>

public class Cannon : MonoBehaviour {

	//Assets
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public Transform tileSpawer = null;

	//Mechanics
	public float cooldown;
	int speed;
	int attack;
	float countdown;
	float countStart;


	//scripts
	GameMechanics GMX;
	Unit unit;

	void Awake(){

		GMX= Camera.main.GetComponent<GameMechanics> ();
		unit = GetComponent<Unit> ();
		speed = unit.speed;
		attack = unit.attack;

	}

	// Use this for initialization
	void Start () {

		countStart = cooldown;
		countdown = countStart;

		Debug.Log ("Name=" + Unit.instance.modelname);
		Debug.Log ("speed=" + speed);
		Debug.Log ("attack=" + speed);
		Debug.Log ("countdown=" + countdown);
		Debug.Log ("countStart=" + countStart);
		
	}
	
	// Update is called once per frame
	void Update () {

		AutoFire ();
		
	}

	void AutoFire(){
	/*

		Automatically shoots every number of cooldown seconds

	 */

		countdown = countdown - Time.deltaTime;
		if (countdown <= 0) {
			countdown = countStart;

			//countdown actions 
			Fire ();
	

		}
	}


	void Fire()
	{
	/*

		Generate projectile and it shoots towards it's target

	 */

		// Create the Bullet from the Bullet Prefab

		//find closest enemy
		Transform aimTarget = FindClosestEnemy();

		//calculate cross product
		Vector3 relativePos = aimTarget.position - bulletSpawn.position;

		//set aim
		Quaternion aimRotation = Quaternion.LookRotation(relativePos, Vector3.up);

		//fire!
		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, aimRotation);

		// Add velocity to the bullet
		bullet.GetComponent<Photon>().speed = speed;
		bullet.GetComponent<Photon> ().damage = attack;
		//bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

	}

	void OnCollisionEnter (Collision col){
	/*

		Handle collision events

	*/

		//if enemy runs into the player, take damage
		if (col.gameObject.tag == "Enemy") {

			//deal damage to player asset based on enemy self attack score
			GetComponent<Unit>().hp -= col.gameObject.GetComponent<Unit>().attack;
		}

		//if we run out of hit points, die or destroy self
		if (GetComponent<Unit>().hp <= 0) {

			//let spawn tile know cannon is self-destroyed but setting false
			tileSpawer.GetComponent<SpawnTile>().isSpawned = false;

			//self-destroy cannon
			GMX.SelfDestruct (gameObject);

		}
	}

	Transform FindClosestEnemy(){
	/*

		Finds closest enemy target
		
	*/

		//get nearest target
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		//loop through all enemies to find nearest
		GameObject closest = null;
		float distance = Mathf.Infinity;

		foreach(GameObject enemy in enemies){
			Vector3 diff = enemy.transform.position - transform.position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = enemy;
				distance = curDistance;
			}
		}

		return closest.transform;

	}
		

}
