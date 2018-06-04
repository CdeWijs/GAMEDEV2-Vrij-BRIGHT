using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Simple : EnemyBaseClass {

    private Transform targetTransform;
    public SpriteRenderer sprR;
    public Slider healthSlider;

    public float chaseRadius;
    public float attackRadius;
   
    public float amplitudeX = 10.0f;
    public float amplitudeY = 5.0f;
    public float omegaX = 1.0f;
    public float omegaY = 1.0f;

    private float index;
   
    new public void Start(){
        base.Start();
        healthSlider.value = 100;
        //sprR = gameObject.GetComponent<SpriteRenderer>();
    }
    new public void Update(){
        //base.Update();
        //EnemyMovement();
    }
    //enemy moves towards target 
    public override void EnemyMovement() {

        float distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));

        if (distanceToPlayer > chaseRadius){

            index += Time.deltaTime;
            float x = amplitudeX * Mathf.Cos(omegaX * index);
            FlipSprite(x);
            //float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * index));
            transform.localPosition = new Vector3(x, transform.position.y, 0);
        }

        else{
            Vector3 moveToPos = new Vector3(playerObject.transform.position.x, transform.position.y, 0);
            transform.position = Vector2.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
        }
    }

    private void FlipSprite(float _x){
        if (_x < -4f){
            sprR.flipX = true;
        }

        else if (_x > 4f){
            sprR.flipX = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.tag == "Player"){
            playerObject.GetComponent<BoyClass>().health -= 10;
        }
    }
}
