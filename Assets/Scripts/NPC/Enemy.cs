using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Behavior behavior;
    public enum Behavior { Alpha, Beta, Gamma, Delta, Epsilon}

    public Transform destinedTarget;

    Transform commandCenter;


    void Awake() {

        GMX = GameObject.Find("GameManager").GetComponent<GameMechanics>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
        commandCenter = GameObject.Find("CommandCenter").transform;

        //navmesh agent and read unit speed initial
        agent.speed = unit.speed;

        behavior = GetRandomRollBehavior();
        //print("random behavior =" + behavior);

        //sets target based on AI preset
        destinedTarget = SetTargetBasedOnBehavior(commandCenter, behavior);

    }


    //switches target based on AI behavior type
    private Transform SetTargetBasedOnBehavior(Transform _destinedTarget, Behavior _behavior) {

        //if behavior is alpha
        if(_behavior == Behavior.Alpha) {
            //it goes for the command center and is a leader
            _destinedTarget = commandCenter;

        } else if(_behavior == Behavior.Beta) {
            //if Beta,  it targets the command center if not the first bullet that hits it (set in collision)
            if(destinedTarget == null) _destinedTarget = commandCenter;
            else _destinedTarget = destinedTarget;

        } else if(_behavior == Behavior.Gamma) {
            //if Gamma, it follows the closest spider buddy it sees unless the command center is closer
            _destinedTarget = GetClostestUnit(_destinedTarget, "Enemy");

        } else if(_behavior == Behavior.Delta) {
            //if Delta, it follows any random cannon unless the command center is closer
            _destinedTarget = GetClostestUnit(_destinedTarget, "Player");

        } else if(_behavior == Behavior.Epsilon) {
            //if Delta, it follows any random cannon unless the command center is closer
            _destinedTarget = GetClostestUnit(_destinedTarget, "EnemyAlpha");

        }

        //print("behavior=" + _behavior.ToString());

        return _destinedTarget;
    }


    //gets closest target if not the destined one
    private Transform GetClostestUnit(Transform _destinedTarget,string tagType) {

        bool isAlpha = false;
        if(tagType == "EnemyAlpha") {
            tagType = "Enemy";
            isAlpha = true;
        }

        List<GameObject> allUnits = GameObject.FindGameObjectsWithTag(tagType).ToList();
        allUnits.Add(_destinedTarget.gameObject);
        allUnits.Remove(gameObject);
        float defaultDistance = 10000000;
        float closestDistance = defaultDistance;

        if(allUnits.Count > 0) {
            foreach(GameObject _unit in allUnits) {

                //measure distance from transform to object
                float distance = (_unit.transform.position - transform.position).sqrMagnitude;
                if(distance < closestDistance) {
                    closestDistance = distance;

                    //if looking for alpha
                    if(isAlpha) {

                        //check the status of enemy and if not, then ignore
                        if (_unit.GetComponent<Enemy>() != null && _unit.GetComponent<Enemy>().behavior == Behavior.Alpha) _destinedTarget = _unit.transform;
                        else
                            closestDistance = defaultDistance;
                    } else
                        _destinedTarget = _unit.transform;   
                }
            }
        }
        return _destinedTarget;
    }

    // Use this for initialization
    void Start () {

        //agent.destination = GameObject.Find("CommandCenter").transform.position;
        agent.destination = destinedTarget.position;

    }


	// Update is called once per frame
	void Update () {

        //move unit on update
		TimeDestruct (3f);

        //set animation speed and gesture to go with navMeshAgent velocity
        speed = Mathf.Lerp(speed,agent.velocity.magnitude,Time.deltaTime * 10);
        animator.SetFloat("WalkD",speed);

        //set destination target on update
        destinedTarget = SetTargetBasedOnBehavior(commandCenter, behavior);
        agent.destination = destinedTarget.position;

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

            //set source to whomever shot the cannon if behavior beta
            if(behavior == Behavior.Beta) {
                print("switching to new target after being shot...");
                destinedTarget = col.gameObject.GetComponent<Photon>().source;
            }

        }

		//if we run out of hit points, die or destroy self
		if (GetComponent<Unit>().hp <= 0) {
            AwardPoints();

            //SelfDestruct ();
            GMX.SelfDestruct(gameObject);

        }

        //if enemy runs into the player, destroy self
        if ( col.gameObject.tag == "CommandCenter") {

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


    //self destrcut after a time period, take out someone within distance
    void TimeDestruct(float destroyDistance){

		selfDestructCountdown = selfDestructCountdown - Time.deltaTime;

		if (selfDestructCountdown<= 0) {

            Transform target = GetClostestUnit(commandCenter,"Player");
            if ((target.position - transform.position).sqrMagnitude < destroyDistance) GMX.SelfDestruct(target.gameObject);
            GMX.SelfDestruct (gameObject);
		}
	}


    //gets random enum value
    static T GetRandomEnum<T>() {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T) A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }


    //random num based on die roll
    public int dieRoll(int diceNum) {

        int val = 0;

        //for every num of dice, roll and add to total value. For loops makes better randomizing if possible
        for (int i = 0; i < diceNum; i++)  val += (Random.Range(0, 6) + 1);

        return val;
    }


    public Behavior GetRandomRollBehavior() {
        Behavior randomBe = Behavior.Alpha;

        // we're going to roll 5 die, one per AI for the best chance, (5 - 30)
        int numRoll = dieRoll(5);

        //50% should be alpha, the other 50, split even by other choices.
        if(numRoll <= 7) randomBe = Behavior.Beta;
        else if(numRoll > 7 && numRoll <= 13) randomBe = Behavior.Delta;
        else if(numRoll > 22 && numRoll <= 27) randomBe = Behavior.Epsilon;
        else if(numRoll > 27) randomBe = Behavior.Gamma;

        print("rolled a :" + numRoll + " AI=" + randomBe.ToString());

        return randomBe;
    }

}
