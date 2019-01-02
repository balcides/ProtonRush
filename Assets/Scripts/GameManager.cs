using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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
	public int playerXPscore;
	public int creditCostLvl2 = 200;
	public int creditCostRepairCmdCenter = 500;

	public float tempTimer;
	public bool enableTempTimer;

	//script
	AssetManager AM;
	GameMechanics GMX;
	Leaderboard leaderboard;

	void Awake(){
		
		AM = GetComponent<AssetManager> ();
		GMX = GetComponent<GameMechanics> ();
		leaderboard = GetComponent<Leaderboard> ();
		playerXP = 400;
		playerXPscore = playerXP;
	}

	// Use this for initialization
	void Start () {
		tempTimer = 10;
	//	leaderboardTimer = 3;
	}
	
	// Update is called once per frame
	void Update () {

		//update xp count in GUI
		AM.xpCounter.GetComponent<Text> ().text = "XP " + playerXP.ToString("00000");
		AM.killCounter.GetComponent<Text> ().text = "Kill " + playerKillCount.ToString("00000");
		AM.xpTotalScore.GetComponent<Text> ().text = "XP Score " + playerXPscore.ToString("00000");

		//enable temp timer for testing if needed
		if (enableTempTimer) {    TempTimer ();   }
	}
		

	void TempTimer(){
	/*

			Created for testing game over and score mechanic

	*/

		//print("Temp timer = " + tempTimer);
		tempTimer -= 1 * Time.deltaTime;
		if(tempTimer <= 0){
			tempTimer = 0;
			GMX.GameOver ();
		}
	}
		

	public void ReloadGame(){
	/*

		Restarts game with retry button by reloading the scene

	*/

		SceneManager.LoadScene("FirstPlayable");
	}
		
}
