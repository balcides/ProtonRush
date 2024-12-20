using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Photon.cs
/// 
/// Attach this to bullet or photon. Meant to handle having it move in one direction
/// 
/// </summary>


public class Photon : MonoBehaviour {

	//Stats
    [Header("Local")]
    public float zLimit;       //bounds by which photons self destruct
    public Vector3 direction;

    [Header("Inherited")]
    public int damage;
    public float speed;
    public Transform source;

    //initialize
    public static Photon instance = null;

    // Reference to the cube platform for photon bounds
    public GameObject platform; 

    //private vars
    private float xMin, xMax, zMin, zMax;
    private Rigidbody photonRigidbody;

	void Awake(){

		//works best when you need to run a method or run a class but not for update info
		instance = this;

        //search for platform 
        platform = GameObject.Find("TerrainPlane");

        //assign rigidbody
        photonRigidbody = GetComponent<Rigidbody>();
	}

    void Start() {

        // Get the bounds of the platform's Renderer or Collider
        Bounds bounds = platform.GetComponent<Renderer>().bounds;

        // Set the bounding box edges
        xMin = bounds.min.x;
        xMax = bounds.max.x;
        zMin = bounds.min.z;
        zMax = bounds.max.z;

        // Start the coroutine to check the bounds
        StartCoroutine(CheckBoundsRoutine(0.1f));
    }
	
    
	// Update is called once per frame
	void FixedUpdate () {


        /*
        * Physics-aware movement, convert direction to world space, then move the Rigidbody's position toward
        * that direction by deltaTime
        * 
        * Note: this can be done with a simple Translate, which we keep here as a fallback example . But 
        * using the Rigidbody's MovePosition is more accurate and physics-aware.
```     *
        * //move this set direction by deltaTime
		*  transform.Translate(direction * Time.fixedDeltaTime * speed);
        */
        Vector3 worldDirection = transform.TransformDirection(direction); 
        Vector3 newPosition = transform.position + worldDirection * Time.fixedDeltaTime * speed;
        photonRigidbody.MovePosition(newPosition);


    }


    //destroy photon after num seconds. Coroutine lightens load on update

    /**
    * <summary>
    * Destroy photon after num seconds. Coroutine lightens load on update
    * </summary>
    * 
    * <param name="num">Number of seconds before photon is destroyed</param>
    */
    private IEnumerator CheckBoundsRoutine(float waitTime = 0.1f) {
        while (true){
            yield return new WaitForSeconds(waitTime);
            Vector3 position = transform.position;

            //check if photon is past bounds of z platform
            if (position.x < xMin || position.x > xMax ||
                position.z < zMin || position.z > zMax){

                //destroy photon
                Destroy(gameObject);
            }
        }
    }
}
