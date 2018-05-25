using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Simple : EnemyBaseClass {

    private Transform targetTransform;
    private SpriteRenderer sprR;

    new public void Start(){
        base.Start();
        sprR = GetComponentInChildren<SpriteRenderer>();
    }
    new public void Update(){
        EnemyMovement();
    }

    //enemy moves towards target 
    public override void EnemyMovement() {
        float xPos = Mathf.Sin(Time.time * 1.25f);
        
        if (xPos < 0){
            sprR.flipX = true;
        }

        else{
            sprR.flipX = false;
        }

        Vector3 moveToPos = new Vector3(transform.position.x + xPos, transform.position.y, 0);
        transform.position = Vector2.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.tag == "Player"){
            playerObject.GetComponent<BoyClass>().health -= 15;
        }
    }
}
