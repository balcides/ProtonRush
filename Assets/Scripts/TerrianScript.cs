using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianScript : MonoBehaviour
{


    GuiManager GUIM;


    private void Awake() {
        GUIM = GameObject.Find("GUIManager").GetComponent<GuiManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver() {
        GUIM.gameInfo.text = "";
    }
}
