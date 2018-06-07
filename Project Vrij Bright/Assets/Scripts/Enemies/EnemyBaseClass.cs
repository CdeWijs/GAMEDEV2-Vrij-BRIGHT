using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// baseclass for enemies to derrive from
/// </summary>
public class EnemyBaseClass : MonoBehaviour {

    public int enemyHealth;
    public float moveSpeed, chaseSpeed;
    public float alertRadius, chaseRadius, attackRadius;

    public Rigidbody2D rigidBody2D;
    public GameObject playerObject;

    public Slider enemyHealthSlider;

    public void Start() {
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyHealthSlider.maxValue = enemyHealth;
        enemyHealthSlider.value = enemyHealth;
    }

    public void Update() {
        EnemyMovement();
        FindPlayer();
        CheckHealth();
    }

    public virtual void TakeDamage(int amount) {
        enemyHealthSlider.value -= amount;
        if (enemyHealthSlider.value <= 0){
            Destroy(gameObject);
        }
    }

    public virtual void EnemyMovement() {
        //add behaviour on derriving enemy
    }

    public virtual void FindPlayer() {
        //add behaviour on derriving enemy
    }

    public virtual void CheckHealth() {
        if (enemyHealth <= 0) {
            Destroy(this.gameObject);
            
        }
    }

}
