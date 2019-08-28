using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{

    public Text gameStatusText;


    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameStatusText.text = "Prepare for attack!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
