using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

    Draws and manages healthbar on all Characters. 
    Works by parenting to any node with CharacterStats Component
      
 */

public class NPCHealthbar : MonoBehaviour
{
    public float rotationZoffset = 0f;
    public RectTransform bar;
    public float deltaSmalls; //does nothing. It's a cool name so I'm keeping
    public bool appearOnHit;
    public float appearOnHitCooldown;

    Unit unit;
    float percentage;

    private Vector3 localScale;
    private float lastAttackTime;


    private void Awake() {
    
        unit = transform.parent.GetComponent<Unit>();
    }


    // Use this for initialization
    void Start() {

        rotationZoffset = 0;
        localScale = transform.localScale;
        transform.LookAt(Camera.main.transform);
        if(appearOnHit) { gameObject.SetActive(false); }
    }


    // Update is called once per frame
    void Update() {

        UpdateHealthBarOnHit();

    }


    private void UpdateHealthBarOnHit() {
    /*

        Draws, updates, and disables healthbar on hit
   
     */

        //get percentage health
        percentage = (((float) unit.hp / (float) unit.maxHp) * 100) - 100;

        //set bar size to that num (left)
        bar.sizeDelta = new Vector2(percentage,87);

        //health bar stares at camera
        Transform cam = Camera.main.transform;
        Vector3 worldUp = Vector3.up;
        transform.LookAt(Camera.main.transform,worldUp);

        //designed to make this appear only when activated
        if(appearOnHit) {

            bool isCooldown = (Time.time - lastAttackTime) < appearOnHitCooldown;

            if(!isCooldown) {

                lastAttackTime = Time.time;
                gameObject.SetActive(false);

            }
        }
    }
}
