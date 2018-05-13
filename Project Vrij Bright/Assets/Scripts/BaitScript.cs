using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script for bait: Player cotroller needs to be referenced in Update
/// </summary>
public class BaitScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private GameObject player;


    private GuardianController guardianController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Guardian");
        guardianController = player.GetComponent<GuardianController>();
        Debug.Log(guardianController);

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
    }

    private void Update()
    {
        if (guardianController.x_active)
        {
            rb2d.gravityScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
        }
    }
}
