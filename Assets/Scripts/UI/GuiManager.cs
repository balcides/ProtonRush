using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{

    public Text gameInfo;
    public Text cryptoCounter;
    public Text killCounter;
    public Text xpScoreCounter;
    public Text gameStatus;
    public Text playerScoreName;
    public Text playerScoreKills;
    public Text playerScoreXpScore;

    public Transform playerScoreBKG;
    public Transform playerScoreSubmitBtn;
    public Transform playerScoreNameInput;
    public Transform tryAgainBtn;



    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInfo.text = "Prepare for attack!";
        playerScoreName.gameObject.GetComponent<Text>().enabled = false;
        playerScoreKills.gameObject.GetComponent<Text>().enabled = false;
        playerScoreXpScore.gameObject.GetComponent<Text>().enabled = false;
        playerScoreBKG.gameObject.SetActive(false);
        playerScoreSubmitBtn.gameObject.SetActive(false);
        playerScoreNameInput.gameObject.SetActive(false);
        tryAgainBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
