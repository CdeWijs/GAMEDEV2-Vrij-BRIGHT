using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class for the lurker type enemy: enemy is invincible while in shadows, but can be attacked when it comes out of hiding by using bait
/// </summary>
public class Enemy_Lurker : EnemyBaseClass {

    public GameObject bait;
    public bool baitOnGround = false;
    public bool isInShadows = true;
    public float attackRadius, chaseRadius;

    private Transform targetTransform;
    private enum LurkerStates { idle, chasePlayer, attack, chaseBait};
    private LurkerStates enemyState;

    new private void Start()
    {
        base.Start();
        bait = GameObject.FindGameObjectWithTag("Bait");
        enemyState = LurkerStates.idle;
    }
    
    //does not call base update because of statemachine
    new private void Update()
    {
        //base.Update();
        FindPlayer();
        FindBait();
        StateMachine(enemyState);
    }

    //enemy moves towards target 
    public override void EnemyMovement()
    {
        Vector3 moveToPos = new Vector3(targetTransform.transform.position.x, transform.position.y, 0);
        transform.position = Vector2.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
       // enemyState = LurkerStates.idle;

    }

    private void AttackTarget()
    {
        
    }

    //checks if player is in range for chasing or attacking
    public override void FindPlayer()
    {
        base.FindPlayer();
        float distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
       
        if (distanceToPlayer < attackRadius)
        {

            enemyState = LurkerStates.attack;
        }

         else if (distanceToPlayer < chaseRadius)
        {
            enemyState = LurkerStates.chasePlayer;
          

        }
    }

    //checks if bait is on the ground and if enemy should come out of hiding
    private void FindBait()
    {
        if (bait.gameObject == null)
        {
            return;
        }
      
        if (Mathf.RoundToInt(bait.transform.position.y) == Mathf.RoundToInt(transform.position.y)) 
        {  
            enemyState = LurkerStates.chaseBait;
       }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bait")
        {
            Destroy(collision.gameObject);
            baitOnGround = false;
            isInShadows = false;
            enemyState = LurkerStates.idle;
        }
    }
    
    //state machine for lurker enemy
    private void StateMachine (LurkerStates state)
    {
        Debug.Log(state);
        switch (state)
        {
            case LurkerStates.idle:

                break;

            case LurkerStates.chaseBait:
                targetTransform = bait.transform;
                EnemyMovement();

                break;
            case LurkerStates.chasePlayer:
                targetTransform = playerObject.transform;
                EnemyMovement();
                break;

            case LurkerStates.attack:
                targetTransform = null;
                break;
            default:
                break;

        }
    }
}
