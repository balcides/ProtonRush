using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

    Attach this to Text prefab and it manages it's scrolling as well as 
    destroy animation   
      
 */

public class ScrollingText : MonoBehaviour {

    public float Duration = 1f;
    public float Speed;

    public Text textMessage;
    private float startTime;

    void Awake() {

        textMessage = transform.GetChild(0).GetComponent<Text>();
        startTime = Time.time;
    }


    // Update is called once per frame
    void Update() {

        ScrollAndDestroy();

    }

    private void ScrollAndDestroy() {
    /*

        Scrolls and destroys

    */
        if(Time.time - startTime < Duration) {

            //Scroll up
            transform.LookAt(Camera.main.transform);
            transform.Translate(Vector3.up * Speed * Time.deltaTime);

        } else {

            //Destroy
            Destroy(gameObject);
        }
    }


    //called by emitter
    public void SetText(string text) => textMessage.text = text;


    //called by emitter
    public void SetColor(Color color) => textMessage.color = color;
}
