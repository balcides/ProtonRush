﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

//AssetManager is any c# script with a refernce to object names called in this code
//GameManager is any c# script with reference to object names called in this code
[RequireComponent(typeof(AssetManager))]
[RequireComponent(typeof(GameManager))]

public class Leaderboard : MonoBehaviour {

    public string defaultNames;
    public List<PlayerScore> playerScore;

    //scripts
    AssetManager AM;
    GameManager GM;

    //Debug
    public bool enableDeleteAllPrefs = false;


    void Awake() {

        AM = GetComponent<AssetManager>();
        GM = GetComponent<GameManager>();
        playerScore = new List<PlayerScore>();

    }

    // Use this for initialization
    void Start() {


        //load player prefs, if none, set default scores
        string[] loadScores = LoadScores();
        UpdateScoreText(loadScores,AM.playerScoreName,AM.playerScoreKills,AM.playerScoreXpScore);

    }


    // Update is called once per frame
    void Update() {

        //deletes prefs if enabled for debugging purposes
        if(enableDeleteAllPrefs) { DeleteAllPrefs(); }
    }


    public void AddScore(List<PlayerScore> playerScore,string Name,int Kills,int TotalXp) {
        /*

            Fun, easy way to add score without crazy Ienumerator stuff

         */

        playerScore.Add(new PlayerScore { name = Name,kills = Kills,totalxp = TotalXp });
    }


    public List<PlayerScore> SortScore(List<PlayerScore> playerScore) {
        /*

            Quick way to sort player score kills, totalxp, and then name

         */

        List<PlayerScore> sortedPlayerScore = playerScore.OrderByDescending(x => x.totalxp).ThenByDescending(x => x.kills).ThenBy(x => x.name).ToList();

        return sortedPlayerScore;
    }


    public List<PlayerScore> TrimScore(List<PlayerScore> playerScore,int maxLimit) {
        /*

            Trims the last item of score if greater than cap number

         */

        //get the number of items in list
        for(int i = playerScore.Count - 1; i >= 0; i--) {
            if(playerScore.Count > maxLimit) {
                playerScore.RemoveAt(i);
            }
        }

        return playerScore;

    }


    List<PlayerScore> DefaultScores(List<PlayerScore> playerScore) {
        /*

            Establish default scores on game start

         */

        AddScore(playerScore,"Anon",1,100);
        AddScore(playerScore,"Anon",200,1300);
        AddScore(playerScore,"Anon",300,1400);
        AddScore(playerScore,"Anon",400,1500);
        AddScore(playerScore,"Anon",500,1550);
        AddScore(playerScore,"Anon",600,1600);
        AddScore(playerScore,"Anon",700,1750);
        AddScore(playerScore,"Anon",800,1800);
        AddScore(playerScore,"Anon",900,1900);
        AddScore(playerScore,"Anon",1000,2500);

        return playerScore;
    }


    public string[] ListToStringScores(List<PlayerScore> playerScore) {
        /*

            Takes player score and outputs then into string format

         */

        string[] scoreText = new string[3];

        foreach(PlayerScore item in playerScore) {
            scoreText[0] = scoreText[0] + item.name + "\n";
            scoreText[1] = scoreText[1] + item.kills.ToString("00000") + "\n";
            scoreText[2] = scoreText[2] + item.totalxp.ToString("00000") + "\n";
        }

        return scoreText;

    }


    public List<PlayerScore> StringToListScores(string[] scores) {
        /*

            Converts string to list format

         */

        string[] names = scores[0].Split();
        string[] kills = scores[1].Split();
        string[] xptotals = scores[2].Split();

        List<PlayerScore> playersScore = new List<PlayerScore>();

        int nameCount = names.Count() - 1;

        //populate the list with string array split
        for(int i = 0; i < nameCount; i++) {
            //print ("nameCount =" + nameCount + " current iterator = " + i);

            //handles blank score in exception
            if(kills[i] == "") {
                kills[i] = "0";
                xptotals[i] = "0";
            }

            //print (names [i] + " " + kills [i] + " " + xptotals [i]);
            AddScore(playersScore,names[i],Convert.ToInt16(kills[i]),Convert.ToInt16(xptotals[i]));
        }

        return playersScore;
    }


    public void UpdateScoreText(string[] scores,Transform nameText,Transform killsText,Transform xpscoresText) {
        /*

            Takes player score array and sets their string to leaderboard layout.
            Attach three text components and a score array

         */

        nameText.GetComponent<Text>().text = "NAME\n" + scores[0];
        killsText.GetComponent<Text>().text = "KILLS\n" + scores[1];
        xpscoresText.GetComponent<Text>().text = "XP SCORE\n" + scores[2];

    }


    public string[] LoadScores(string nameKey = "Name",string killsKey = "Kills",string xpTotalKey = "XpTotal") {
        /*

            Loads scores and sorts from player prefs. If empty, loads default list

         */

        //set default scores 
        //TODO: It's maybe more efficient to make default scores in string array instead of list but making it from a list
        //		just seems easier (format friendly)
        List<PlayerScore> defaultScores = new List<PlayerScore>();
        defaultScores = DefaultScores(defaultScores);
        defaultScores = SortScore(defaultScores);

        //create a load scores that's empty to use for player
        //List<PlayerScore> loadScores = new List<PlayerScore>();

        //convert default list to strings
        string[] defaultScoresToString = ListToStringScores(defaultScores);

        //get player pref key, if not, add a default list
        string names = PlayerPrefs.GetString(nameKey,defaultScoresToString[0]);
        string kills = PlayerPrefs.GetString(killsKey,defaultScoresToString[1]);
        string xptotal = PlayerPrefs.GetString(xpTotalKey,defaultScoresToString[2]);

        //use player pref scores from string and convert to list
        string[] loadedPrefScores = new string[] { names,kills,xptotal };
        //loadScores = StringToListScores (loadPrefScores);

        //sort scores and trim to limit
        //List<PlayerScore> sortedPlayerScore = SortScore(loadScores);
        //List<PlayerScore> trimScore = TrimScore (sortedPlayerScore, 10);

        //convert sorted and trimmed list scores to string format
        //string[] scores = ListToStringScores(trimScore);

        //return scores;
        return loadedPrefScores;
    }


    public void SaveScores(string[] scores,string nameKey = "Name",string killsKey = "Kills",string xpTotalKey = "XpTotal") {
        /*

            Saves score from string format into playerPrefs

         */
        //load scores into player prefs
        PlayerPrefs.SetString(nameKey,scores[0]);
        PlayerPrefs.SetString(killsKey,scores[1]);
        PlayerPrefs.SetString(xpTotalKey,scores[2]);
    }

    public void SubmitScores() {
        /*

            Designed to submit scores at the press of a button.

        */

        //create list based on current scores
        List<PlayerScore> playerScore = new List<PlayerScore>();
        string[] loadScores = LoadScores();
        playerScore = StringToListScores(loadScores);

        //Add score from input
        AddScore(playerScore,AM.playerScoreNameInput.GetComponent<InputField>().text,GM.playerKillCount,GM.playerXPscore);

        //sort the scores by kills, total, and name
        List<PlayerScore> sortedPlayerScore = SortScore(playerScore);

        //remove the last item on the list
        List<PlayerScore> trimScore = TrimScore(sortedPlayerScore,10);

        //convert list to string
        string[] scores = ListToStringScores(trimScore);

        //display results and update text
        UpdateScoreText(scores,AM.playerScoreName,AM.playerScoreKills,AM.playerScoreXpScore);

        //save to playerPrefs
        SaveScores(scores);

        //enable try again button (optional)
        AM.tryAgainBtn.gameObject.SetActive(true);

    }

    public void LoadLeaderboard() {
        /*

            Once the game is over, check the high scores

        */

        //Board.thisExample; // works!
        // disable game over text
        AM.gameStatus.GetComponent<Text>().enabled = false;

        //enable board
        AM.playerScoreName.GetComponent<Text>().enabled = true;
        AM.playerScoreKills.GetComponent<Text>().enabled = true;
        AM.playerScoreXpScore.GetComponent<Text>().enabled = true;
        AM.playerScoreBKG.gameObject.SetActive(true);

        //load high scores and update text
        string[] loadScores = LoadScores();
        UpdateScoreText(loadScores,AM.playerScoreName,AM.playerScoreKills,AM.playerScoreXpScore);

        //convert scores to list
        List<PlayerScore> playerscore = new List<PlayerScore>();
        playerscore = StringToListScores(loadScores);

        //get value of the last item in list
        int lowestTotalXP = playerscore[playerscore.Count - 1].totalxp;

        //if kill count is greater, prompt player input
        if(GM.playerXPscore > lowestTotalXP) {
            //print ("player kill count is greater than lowest = " + playerXPscore + " > " + lowestTotalXP);

            //enable input
            AM.playerScoreNameInput.gameObject.SetActive(true);
            AM.playerScoreSubmitBtn.gameObject.SetActive(true);

            //Note: sort, trim, and save handled by SubmitScores() when button is enabled

        } else {

            //enable try again button (optional)
            AM.tryAgainBtn.gameObject.SetActive(true);

        }
    }

    public void DeleteAllPrefs() {

        //nuke or reset prefs if need be
        PlayerPrefs.DeleteAll();
    }

}


public class PlayerScore {
    /*

        List class designed to hold Key values for a leaderboard ala Star Kingdom style

    */

    public string name { get; set; }
    public int kills { get; set; }
    public int totalxp { get; set; }

}



