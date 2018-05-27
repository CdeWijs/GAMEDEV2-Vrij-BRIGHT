using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script for bait: Player cotroller needs to be referenced in Update
/// </summary>
public class BaitScript : MonoBehaviour {
    private Rigidbody2D rb2d;
    
    private GameObject player;


    private GuardianController guardianController;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Guardian");
        guardianController = player.GetComponent<GuardianController>();
        Debug.Log(guardianController);

        
       // rb2d.gravityScale = 0;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (guardianController.x_active || Input.GetKeyDown(KeyCode.E))
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
            gameObject.AddComponent<BoxCollider2D>();
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
    }
}
