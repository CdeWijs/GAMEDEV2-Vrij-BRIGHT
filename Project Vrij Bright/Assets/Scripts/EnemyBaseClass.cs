using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// baseclass for enemies to derrive from
/// </summary>
public class EnemyBaseClass : MonoBehaviour {

    public int enemyHealth;
    public float moveSpeed;

    public Rigidbody2D rb2d;
    public GameObject playerObject;

    public void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        EnemyMovement();
        FindPlayer();
        CheckHealth();
    }

    public virtual void TakeDamage()
    {
        //add behaviour on derriving enemy
    }

    public virtual void EnemyMovement()
    {
        //add behaviour on derriving enemy
    }

    public virtual void FindPlayer()
    {
        //add behaviour on derriving enemy
    }

    public virtual void CheckHealth()
    {
        if (enemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
