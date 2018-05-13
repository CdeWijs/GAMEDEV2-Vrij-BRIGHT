using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script checks player health and  
/// </summary>
public class BoyClass : MonoBehaviour {

    public int health;
    public int attackDamage;
    public int moveSpeed;

    //temporary slider ui
    public Slider playerHealthSlider;

    public static bool boyIsScared = false;

    private BoyController controllerScript;

    private void Start()
    {
        controllerScript = this.gameObject.GetComponent<BoyController>();
        playerHealthSlider.value = 100;
    }

    private void Update()
    {
        playerHealthSlider.value = health;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //behaviour for when player is scared
        if (collision.tag == "Shadow")
        boyIsScared = true;
        controllerScript.speed = 2;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //behaviour to restore player
        if (collision.transform.tag == "Shadow")
        {
            boyIsScared = false;
            controllerScript.speed = 8;
        }
    }
}
