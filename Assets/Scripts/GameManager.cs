using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	float tempTimer;
	float leaderboardTimer;

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
		leaderboardTimer = 3;
	}
	
	// Update is called once per frame
	void Update () {

		//update xp count in GUI
		AM.xpCounter.GetComponent<Text> ().text = "XP " + playerXP.ToString("00000");
		AM.killCounter.GetComponent<Text> ().text = "Kill " + playerKillCount.ToString("00000");
		AM.xpTotalScore.GetComponent<Text> ().text = "XP Score " + playerXPscore.ToString("00000");

		//created for testing game over and score mechanic
		//print("Temp timer = " + tempTimer);
		tempTimer -= 1 * Time.deltaTime;
		if(tempTimer <= 0){
			tempTimer = 0;
			GMX.GameOver ();

			//start leaderboard timer
			leaderboardTimer -= 1 * Time.deltaTime;
			if (leaderboardTimer <= 0) {
				leaderboardTimer = 0;
				LoadLeaderboard ();
			}
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

		//enable input
		AM.playerScoreBKG.gameObject.SetActive(true);
		AM.playerScoreNameInput.gameObject.SetActive (true);
		AM.playerScoreSubmitBtn.gameObject.SetActive (true);

		//if the kill count is greater than the smallest value in the highscore 
		//prompt player name input
		//load high scores


		//enable try again button (optional)
	}


	public void PrintTest(){
		print ("button works!");
	}


	public void SubmitScores(){
	/*

		Designed to submit scores at the press of a button.

	*/
		//take the current score
		//submit to leaderboard
		//get player scores by loading player prefs to list
		//string prefNames = PlayerPrefs.GetString("Name", leaderboard.defaultNames);

		//take pref names and add to list
		//string[] names = prefNames.Split('\n');

		//create list based on current scores
		List<PlayerScore> playerScore = new List<PlayerScore>();
		playerScore = leaderboard.playerScore;

		//List<Leaderboard.PlayerScore> playersScore = new List<Leaderboard.PlayerScore> ();
		leaderboard.AddScore (playerScore, AM.playerScoreNameInput.GetComponent<InputField> ().text, playerKillCount, playerXPscore);

		//sort the scores by kills, total, and name
		//List<PlayerScore> sortedPlayerScore = playersScore.OrderBy(x => x.kills).ThenBy(x => x.totalxp).ThenBy(x => x.name).ToList();
		List<PlayerScore> sortedPlayerScore = leaderboard.SortScore(playerScore);

		//set scores (TEST)
		string[] scores = leaderboard.ScoresToString(sortedPlayerScore) ;
		leaderboard.UpdateScoreText (scores, AM.playerScoreName, AM.playerScoreKills, AM.playerScoreXpScore);

		//remove the last item on the list
		//save to playerPrefs

	}
		
}
