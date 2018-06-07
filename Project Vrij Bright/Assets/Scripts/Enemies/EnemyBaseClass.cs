using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// baseclass for enemies to derrive from
/// </summary>
public class EnemyBaseClass : MonoBehaviour {

    public enum EnemyStates {
        IDLE,
        ALERT,
        CHASE,
        ATTACK,
        DEAD
    }

    public EnemyStates currentState;
    public int enemyHealth;
    public float moveSpeed, chaseSpeed;
    public float alertRadius, chaseRadius, attackRadius;
    public float attackRate = 1f;
    public float nextAttack;

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
        if (playerObject == null) {
            currentState = EnemyStates.IDLE;
        } else {
            float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
            if (_distanceToPlayer < attackRadius) {
                currentState = EnemyStates.ATTACK;
            } else if (_distanceToPlayer < chaseRadius) {
                currentState = EnemyStates.CHASE;
            } else if (_distanceToPlayer < alertRadius) {
                currentState = EnemyStates.ALERT;
            } else {
                currentState = EnemyStates.IDLE;
            }
        }
    }

    public virtual void CheckHealth() {
        if (enemyHealth <= 0) {
            Destroy(this.gameObject);
            
        }
    }

    public virtual void ChaseTarget(Transform _target) {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, chaseSpeed * Time.deltaTime);
    }

    public virtual void Attack() {
        if (Time.time > nextAttack) {
            playerObject.GetComponent<BoyClass>().health -= 1;
            nextAttack = Time.time + attackRate;
            Debug.Log("attack!");
        }
    }

}
