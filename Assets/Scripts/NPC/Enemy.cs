﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float speed;
    public Animator animator;

    //mechanics
    public float selfDestructCountdown;

	//scripts
	//AssetManager AM;
	GameMechanics GMX;
	GameManager GM;
    NavMeshAgent agent;
    Unit unit;


	void Awake(){

		GMX = GameObject.Find("GameManager").GetComponent<GameMechanics> ();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();

        //navmesh agent and read unit speed initial
        agent.speed = unit.speed;

    }


    // Use this for initialization
    void Start () {

        agent.destination = GameObject.Find("CommandCenter").transform.position;

    }


	// Update is called once per frame
	void Update () {

        //move unit on update
		TimeDestruct ();


        speed = Mathf.Lerp(speed,agent.velocity.magnitude,Time.deltaTime * 10);
        animator.SetFloat("WalkD",speed);

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
            AwardPoints();

            //SelfDestruct ();
            GMX.SelfDestruct(gameObject);

        }

        //if enemy runs into the player, destroy self
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "CommandCenter") {

            //Award player XP for kill
            AwardPoints();

            //SelfDestruct();
            GMX.SelfDestruct (gameObject);

			//deal damage to player asset based on enemy self attack score
			col.gameObject.GetComponent<Unit>().hp -= GetComponent<Unit>().attack;
		}
	}


    //Award player XP for kill
    private void AwardPoints() {

        GM.playerKillCount += 1;
        GM.crypto += GetComponent<Unit>().cryptoXP;
        GM.playerXPscore += GetComponent<Unit>().cryptoXP;

        GM.enemiesDefeatedPerRound += 1;
    }


    void Move(){
	/*

		Auto moves the enemy asset (Legacy, there just in case. Delete if no longer needed)

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
