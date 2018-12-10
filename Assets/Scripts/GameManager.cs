using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Game manager.
/// 
/// Manages main rules of the game overall such as tracking points, win, lose, etc. 
/// 
/// </summary>

public class GameManager : MonoBehaviour {

	//values
	public int playerXP;
	public int playerKillCount;
	public int creditCostLvl2 = 200;

	//script
	AssetManager AM;

	void Awake(){
		
		AM = GetComponent<AssetManager> ();
		playerXP = 400;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//update xp count in GUI
		AM.xpCounter.GetComponent<Text> ().text = "XP " + playerXP.ToString("00000");
		AM.killCounter.GetComponent<Text> ().text = "Kill " + playerKillCount.ToString("00000");
		
	}
		
}
