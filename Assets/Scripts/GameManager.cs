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
	public int crypto;
	public int playerKillCount;
	public int playerXPscore;
	public int creditCostLvl2 = 200;
	public int creditCostRepairCmdCenter = 500;

	public float tempTimer;
	public bool enableTempTimer;

	//script
	GameMechanics GMX;
    GuiManager GUIM;
    Leaderboard leaderboard;

    public int sessionID;

	void Awake(){	
		GMX = GetComponent<GameMechanics> ();
        GUIM = GameObject.Find("GUIManager").GetComponent<GuiManager>();
		crypto = 400;
		playerXPscore = crypto;
	}


	// Use this for initialization
	void Start () {
		tempTimer = 3f;
        //	leaderboardTimer = 3;

        //load and iterate on latest session ID
        sessionID = SessionID();
        print("sessionID =" + sessionID);
	}


	// Update is called once per frame
	void Update () {

		//update xp count in GUI
		GUIM.cryptoCounter.text = "Crypto: " + crypto.ToString("00000");
        GUIM.xpScoreCounter.text = "Score: " + playerXPscore.ToString("00000");
        GUIM.killCounter.text = "Kill: " + playerKillCount.ToString("00000");

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


    //gets the latest session ID and iterates it
    public int SessionID() {

        string key = "SessionID";
        int session = PlayerPrefs.GetInt(key, 0);
        PlayerPrefs.SetInt(key,(session+1));
        return session;
    }
		

    //Load level on name for buttons. Easy convenience
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);


    //handles input, updates, and loading of leaderboard level
    //Note: We could Dimiter this, but I felt breaking the connections in Unity would cause a ruckus, hardcoding ref makes easier tracking
    public void LoadScoreEntryOnGameOver() {

        //get input entry
        //if blank, make it "Player" + SessionID

        //update the scores with the new entry, sort, and save
        leaderboard.SubmitScores();

        //load leaderboad level
        LoadScene("Leaderboard");
    }
}
