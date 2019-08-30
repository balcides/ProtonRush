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

    public Camera[] cameras;
    public int camIndex;

    private void Awake() {
        cameras = GameObject.Find("Cameras").transform.GetComponentsInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInfo.text = "Prepare for attack!";

        //current camera is index
        CycleCameras();
    }

    // Update is called once per frame
    void Update()
    {
        //print(playerScoreNameInput.GetComponent<InputField>().text);
    }


    //Cycles cameras at the touch of a button
    public void CycleCameras() {

        print("cycling camera...");

        //grab all the cameras in an array (from parent transform), get the index, and for every camera, 
        for(int i = 0; i < cameras.Length; i++) {

            //if it matches index, set enable
            if(i == camIndex) {
                cameras[i].gameObject.SetActive(true);
                cameras[i].transform.tag = "MainCamera";
            }

            //else disable
            else {
                cameras[i].gameObject.SetActive(false);
                cameras[i].transform.tag = "Untagged";
            }
        }

        //if index is greater than length, reset index
        int camLength = (cameras.Length-1);
        if(camIndex >= camLength) camIndex = 0;

        //else iterate index
        else camIndex++;
    }
}
