using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEraser : MonoBehaviour {

    public float moveSpeed = 0;
    private float normalSpeed = 0;

    private void Update() {
        moveSpeed += 0.2f * Time.deltaTime;
        
        float step = moveSpeed * Time.deltaTime;
        Vector3 target = new Vector3(transform.position.x + step, transform.position.y, transform.position.z);
        transform.position = target;
        }


    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.tag == "Player") {
            collision.gameObject.GetComponent<BoyClass>().health--;
            }
        }


    //Change player properties on trigger enter
    private void OnTriggerEnter2D(Collider2D collision) {
        //behaviour for when player is scared
        if (collision.tag == "GravityWell" && this.gameObject.layer != 14) {
           
            PhysicsScript.GravityIncrease(this.gameObject, 0.5f, 1.5f);
            normalSpeed = moveSpeed;
            moveSpeed = PhysicsScript.EffectedFloat(moveSpeed, 2);
            }
              //makes player visible when entering mirror in mirrorworld
        
             else if (collision.tag == "Inversion") {
            normalSpeed = moveSpeed;
            moveSpeed = PhysicsScript.EffectedFloat(moveSpeed, 0.1f);
            }
        }

    //Restore player properties on trigger exit 
    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.tag == "GravityWell") {
            moveSpeed = normalSpeed;
            } else if (collision.tag == "Inversion") {
            moveSpeed = normalSpeed;
            }
        //makes player invisible when leaving in mirrorworld
        }


    }
