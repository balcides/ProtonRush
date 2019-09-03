using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

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

    public int round;
    public int sessionID;
    public bool isGameOver;
    public bool isCamAutoSwitch;

    [Header("Game Timers")]
    public float roundsTextTimer;
    public float spawnTimer;
    public float cooldownTimer;
    public float setNextRoundTimer;
    public float cameraSwitchTimer;

    private float roundsTextTimerMax;
    private float spawnTimerMax;
    private float cooldownTimerMax;
    private float setNextRoundTimerMax;
    private float cameraSwitchTimerMax;

    private GameObject portalSpawners;
    public float tempTimer;
	public bool isTempTimer;

    [Header("Misc")]
    string roundsTextMsg;
    public int enemiesDefeatedPerRound;

    //script
    GameMechanics GMX;
    GuiManager GUIM;
    Leaderboard leaderboard;

    float zerop = 0.0001f;




	void Awake(){	
		GMX = GetComponent<GameMechanics> ();
        GUIM = GameObject.Find("GUIManager").GetComponent<GuiManager>();
        leaderboard = GameObject.Find("LeaderboardManager").GetComponent<Leaderboard>();
		crypto = 400;
		playerXPscore = crypto;
        portalSpawners = GameObject.Find("PortalSpawners");
	}


	// Use this for initialization
	void Start () {
        tempTimer = 3f;
        //	leaderboardTimer = 3;

        //load and iterate on latest session ID
        sessionID = SessionID();
        print("sessionID =" + sessionID);
        round = 1;

        //update timers
        roundsTextTimerMax = roundsTextTimer;
        spawnTimerMax = spawnTimer;
        cooldownTimerMax = cooldownTimer;
        setNextRoundTimerMax = setNextRoundTimer;
        cameraSwitchTimerMax = cameraSwitchTimer;
        ResetTimers();

        //disable spawners
        portalSpawners.SetActive(false);
    }


    //resets timers to their Max values
    //TODO: Dimiter this (maybe)
    private void ResetTimers() {
        roundsTextTimer = roundsTextTimerMax;
        spawnTimer = spawnTimerMax + roundsTextTimer;
        cooldownTimer = cooldownTimerMax + spawnTimer;
        setNextRoundTimer = setNextRoundTimerMax + cooldownTimer;
    }


    // Update is called once per frame
    void Update () {

        //update xp count in GUI
        GUIM.cryptoCounter.text = "Crypto: " + crypto.ToString("00000");
        GUIM.xpScoreCounter.text = "Score: " + playerXPscore.ToString("00000");
        GUIM.killCounter.text = "Kill: " + playerKillCount.ToString("00000");

        //Runs any temp scripts within timer
        tempTimer = RunTempTimerScripts(isTempTimer, tempTimer);

        //Runs full rounds loop
        if(!isGameOver) RunGameLoopRounds();


    }


    private void RunGameLoopRounds() {

        string roundsSubtitleTextMsg = "";

        //run all timers countdown
        roundsTextTimer = GTimer(roundsTextTimer);
        spawnTimer = GTimer(spawnTimer);
        cooldownTimer = GTimer(cooldownTimer);
        setNextRoundTimer = GTimer(setNextRoundTimer);
        cameraSwitchTimer = GTimer(cameraSwitchTimer);

        //Rounds event:   display round 1 for x num of seconds
        if(roundsTextTimer > zerop && spawnTimer > zerop && cooldownTimer > zerop && setNextRoundTimer > zerop) {
            roundsTextMsg = "Round " + round;
            roundsSubtitleTextMsg = "get ready";
            GUIM.gameStatusTitle.gameObject.SetActive(true);
            GUIM.gameStatusTitleBKG.gameObject.SetActive(true);
            enemiesDefeatedPerRound = 0;

            //Spawn event: start spawning for x num of seconds
        } else if(roundsTextTimer < zerop && spawnTimer < zerop && roundsTextTimer < zerop && cooldownTimer > zerop && setNextRoundTimer > zerop) {
            roundsSubtitleTextMsg = "Round" + round + "...incoming";
            roundsTextMsg = "spawning units!";
            portalSpawners.SetActive(true);
            GUIM.gameStatusTitle.gameObject.SetActive(false);
            GUIM.gameStatusTitleBKG.gameObject.SetActive(false);

            //Cooldown begins: for x num of seconds followed by countdown text        
        } else if(roundsTextTimer < zerop && spawnTimer < zerop && roundsTextTimer < zerop && cooldownTimer < zerop && setNextRoundTimer > zerop) {
            roundsTextMsg = "Enemy retreat...";
            roundsSubtitleTextMsg = enemiesDefeatedPerRound + " Defeated. Prepare to fortify...";
            portalSpawners.SetActive(false);

            //Reset event: reset all timers after cooldown for x num of seconds
        } else if(roundsTextTimer < zerop && spawnTimer < zerop && roundsTextTimer < zerop && cooldownTimer < zerop && setNextRoundTimer < zerop) {

            //next round begins after cooldown
            ResetTimers();
            GUIM.gameStatusTitle.gameObject.SetActive(false);
            GUIM.gameStatusTitleBKG.gameObject.SetActive(false);
            round++;


        } else {
            
            roundsTextMsg = "Attack in " + Convert.ToString(Mathf.Round(spawnTimer));
            roundsSubtitleTextMsg = roundsTextMsg;
        }

        //update messages
        //print(roundsTextMsg);
        GUIM.gameStatusTitle.text = roundsTextMsg;
        GUIM.gameStatusSubtitle.text = roundsSubtitleTextMsg;

        //swtich cameras every xNum of seconds
        if(isCamAutoSwitch && cameraSwitchTimer < zerop) {
            GUIM.CycleCameras();
            cameraSwitchTimer = cameraSwitchTimerMax;
        }
    }


    //Simple timer handle
    float GTimer(float xTime) {
        xTime -= 1 * Time.deltaTime;
        if (xTime <= 0) xTime = 0;
        return xTime;
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
    public void LoadLeaderboardEntryOnGameOver() {

        //update the scores with the new entry, sort, and save
        leaderboard.SubmitScores();

        //load leaderboad level
        LoadScene("Leaderboard");
    }


    //enable temp timer for testing if needed
    private float RunTempTimerScripts(bool _isTempTimer,float _tempTimer) {

        if(_isTempTimer) {
            _tempTimer = GTimer(_tempTimer);
            if(_tempTimer < zerop) TempScripts();
        }

        return _tempTimer;
    }


    //Run temp scripts here based on timer
    void TempScripts() {

        GMX.GameOver();
    }
}
