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


	public void LoadLeaderboard(){
		//Once the game is over, check the high scores
		//Board.thisExample; // works!
		// disable game over text
		AM.gameStatus.GetComponent<Text>().enabled = false;

		//enable board
		AM.playerScoreName.GetComponent<Text>().enabled = true;
		AM.playerScoreKills.GetComponent<Text>().enabled = true;
		AM.playerScoreXpScore.GetComponent<Text>().enabled = true;
		AM.playerScoreBKG.gameObject.SetActive(true);

		//load high scores and update text
		string[] loadScores = leaderboard.LoadScores ();
		leaderboard.UpdateScoreText (loadScores, AM.playerScoreName, AM.playerScoreKills, AM.playerScoreXpScore);

		//convert scores to list
		List<PlayerScore> playerscore = new List<PlayerScore>();
		playerscore = leaderboard.StringToListScores (loadScores);

		//get value of the last item in list
		int lowestTotalXP = playerscore [playerscore.Count - 1].totalxp;

		//if kill count is greater, prompt player input
		if (playerXPscore > lowestTotalXP) {
			//print ("player kill count is greater than lowest = " + playerXPscore + " > " + lowestTotalXP);

			//enable input
			AM.playerScoreNameInput.gameObject.SetActive (true);
			AM.playerScoreSubmitBtn.gameObject.SetActive (true);

			//Note: sort, trim, and save handled by SubmitScores() when button is enabled

		} else {
			
			//enable try again button (optional)
			AM.tryAgainBtn.gameObject.SetActive(true);

		}


	}


	public void SubmitScores(){
	/*

		Designed to submit scores at the press of a button.

	*/

		//create list based on current scores
		List<PlayerScore> playerScore = new List<PlayerScore>();
		string[] loadScores = leaderboard.LoadScores ();
		playerScore = leaderboard.StringToListScores (loadScores);

		//Add score from input
		leaderboard.AddScore (playerScore, AM.playerScoreNameInput.GetComponent<InputField> ().text, playerKillCount, playerXPscore);

		//sort the scores by kills, total, and name
		List<PlayerScore> sortedPlayerScore = leaderboard.SortScore(playerScore);

		//remove the last item on the list
		List<PlayerScore> trimScore = leaderboard.TrimScore (sortedPlayerScore, 10);

		//convert list to string
		string[] scores = leaderboard.ListToStringScores(trimScore);

		//display results and update text
		leaderboard.UpdateScoreText (scores, AM.playerScoreName, AM.playerScoreKills, AM.playerScoreXpScore);

		//save to playerPrefs
		leaderboard.SaveScores(scores);

		//enable try again button (optional)
		AM.tryAgainBtn.gameObject.SetActive(true);

	}


	public void ReloadGame(){
	/*

		Restarts game with retry button by reloading the scene

	*/

		SceneManager.LoadScene("FirstPlayable");
	}
		
}
