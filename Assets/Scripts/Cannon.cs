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
    GameManager GM;
    AssetManager AM;
    Unit unit;
    GuiManager GUIM;

    private string statusMessage;

    void Awake() {

        GMX = GameObject.Find("GameManager").GetComponent<GameMechanics>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        GUIM = GameObject.Find("GUIManager").GetComponent<GuiManager>();

        unit = GetComponent<Unit>();
        speed = unit.speed;
        attack = unit.attack;

    }


    // Use this for initialization
    void Start() {

        countStart = cooldown;
        countdown = countStart;
        statusMessage = "Cannon: Upgrade to next level= " + GM.creditCostLvl2 + " Crypto";
    }


    // Update is called once per frame
    void Update() {

        AutoFire();
    }


    void AutoFire() {
        /*

            Automatically shoots every number of cooldown seconds

         */


        countdown = countdown - Time.deltaTime;
        if(countdown <= 0) {
            countdown = countStart;

            //countdown actions 
            Fire();
        }
    }


    void Fire() {
        /*

            Generate projectile and it shoots towards it's target

         */
        Quaternion rotateDir = Quaternion.Euler(Vector3.forward);

        //find closest enemy
        Transform aimTarget = FindClosestEnemy();

        //if there's an aim target, get aim rotation
        if(aimTarget) {

            //calculate cross product
            Vector3 relativePos = aimTarget.position - bulletSpawn.position;

            //set aim
            Quaternion aimRotation = Quaternion.LookRotation(relativePos,Vector3.up);
            rotateDir = aimRotation;

            //fire!
            var bullet = (GameObject) Instantiate(bulletPrefab,bulletSpawn.position,rotateDir);

            // Add velocity to the bullet
            bullet.GetComponent<Photon>().speed = speed;
            bullet.GetComponent<Photon>().damage = attack;
            //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        }

    }


    void OnCollisionEnter(Collision col) {
        /*

            Handle collision events

        */

        //if enemy runs into the player, take damage
        if(col.gameObject.tag == "Enemy") {

            //deal damage to player asset based on enemy self attack score
            GetComponent<Unit>().hp -= col.gameObject.GetComponent<Unit>().attack;
        }

        //if we run out of hit points, die or destroy self
        if(GetComponent<Unit>().hp <= 0) {

            //let spawn tile know cannon is self-destroyed but setting false
            tileSpawer.GetComponent<SpawnTile>().isSpawned = false;

            //self-destroy cannon
            GMX.SelfDestruct(gameObject);

        }
    }


    Transform FindClosestEnemy() {
        /*

            Finds closest enemy target

        */

        //get nearest target
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //loop through all enemies to find nearest
        GameObject closest = null;
        float distance = Mathf.Infinity;

        //Calculate if any enemies found, else return null
        if(enemies.Any()) {

            foreach(GameObject enemy in enemies) {
                Vector3 diff = enemy.transform.position - transform.position;
                float curDistance = diff.sqrMagnitude;

                if(curDistance < distance) {
                    closest = enemy;
                    distance = curDistance;
                }
            }

            return closest.transform;

            //return nothing if no enemies found
        } else {
            return null;
        }
    }


    void OnMouseDown() {
        /*

            Upgrade unit on click

         */
        //TODO: used also in SpawnTile.cs, should be a global method. Level2Credits should be refactored
        //look at num of credits
        int numOfCredits = GM.crypto;
        Transform tileSpawnAsset = tileSpawer.GetComponent<SpawnTile>().spawnAsset;

        //set default status message
        statusMessage = "Cannon: Upgrade to next level= " + GM.creditCostLvl2 + " Crypto";

        //if there's enough, convert to level 2
        if(numOfCredits >= GM.creditCostLvl2) {

            //if it's a level 1 cannon, convert to level 2, else, it's maxed out
            if(tileSpawnAsset.GetComponent<Unit>().modelname == "photonSE") {

                //set the tile's new asset for the next level
                tileSpawer.GetComponent<SpawnTile>().spawnAsset = AM.cannons[1];

                //spawn asset after setting prefab
                tileSpawer.GetComponent<SpawnTile>().SpawnAsset();

                //reduce credits 
                GM.crypto -= GM.creditCostLvl2;

                //destroy current one
                GMX.SelfDestruct(gameObject);

            } else {
                print("it's already a level 2 cannon.");
                statusMessage = "it's already a level 2 cannon.";

            }

        } else {

            //TODO: This condition is called again under 'if(numOfCredits..' and should probably be refactored to be called once
            if(tileSpawnAsset.GetComponent<Unit>().modelname == "photonSE2") {
                print("Unit maxed out!");
                statusMessage = "Unit maxed out!";

            } else {
                //else warn you need more credits
                print("Not enough XP to purchase cannon. Must have" + (GMX.CreditsRemaining(GM.creditCostLvl2)) + " credits to purchase");
                statusMessage = "Not enough XP to purchase cannon. Must have " + (GMX.CreditsRemaining(GM.creditCostLvl2)) + " credits to purchase";
                //"Not enough XP to purchase cannon. Must have " + (GMX.creditsRemaining(GM.creditCostLvl2)) + " credits to purchase";

            }
        }

        GUIM.gameInfo.text = statusMessage;
    }


    private void OnMouseOver() {

        if(GetComponent<Unit>().level == 2) statusMessage = "Unit maxed out!";
        GUIM.gameInfo.text = statusMessage;
    }
}
