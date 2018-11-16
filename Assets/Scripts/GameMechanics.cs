using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// GameMechanics.cs
/// 
/// Manages common elements of gameplay to keep Game Manager about rules and consequences
/// while mechanics deals with common things like explosion on destroy for all units and things like that. Common game elements per units
/// 
/// note: you may want to attach this to camera alongside the GameManager.cs
/// 
/// </summary>

public class GameMechanics : MonoBehaviour {

	//scripts
	AssetManager AM;

	void Awake(){

		AM = GetComponent<AssetManager> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void SelfDestruct(GameObject asset){
	/*

		Asset destroys and all things thereof

	*/

		//release explosion effect
		var explosion = (GameObject)Instantiate (AM.explosion.gameObject, asset.transform.position, asset.transform.rotation);

		//destroy self 
		Destroy (asset.gameObject);


	}

}
