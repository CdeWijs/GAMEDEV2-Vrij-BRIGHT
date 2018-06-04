using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour {
    CharacterController characterController;
    Vector3 moveDirection;
    float gravity = 10f;

    public Transform raycastpos;

    private Vector3 MoveDir() {
        if (Grounded()) {
            moveDirection = new Vector3(0, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= 1.5f;

            if (Input.GetKeyDown(KeyCode.X)) {
                moveDirection.y = 10f;
            }
        }

        if (!Grounded()) {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        return moveDirection;
        //transform.Translate(moveDirection * Time.deltaTime);
    }

    private bool Grounded() {
        GameObject hitObject = RayCaster2D(raycastpos.position, Vector2.down, 0.1f);
        if (hitObject != null) {
            if (hitObject.tag == "Ground") {
                Debug.Log("grounded");
                return true;
            }
        }
        Debug.Log("not grounded");
        return false;
    }

    private GameObject RayCaster2D(Vector2 _origin, Vector2 _dir, float _dist) {
        RaycastHit2D hit = Physics2D.Raycast(_origin, _dir, _dist);
        if (hit) {
            return hit.transform.gameObject;
        }

        return null;
    }

    private void CollisionDetection() {
        float dir = Mathf.Sign(moveDirection.x);
        GameObject hitObject = RayCaster2D(raycastpos.position, Vector2.right * dir, 0.1f);
        if (hitObject != null) {
            Debug.Log(hitObject.name);
        }
    }
}