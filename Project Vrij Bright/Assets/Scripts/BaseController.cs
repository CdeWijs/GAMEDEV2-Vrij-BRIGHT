using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    public const float NormalSpeed = 8;
    public const float ScaredSpeed = 2;
    public const float JumpForce = 10;

    public ControllerInput connectedController;
    public bool a_active;
    public bool b_active;
    public bool x_active;
    public bool y_active;
    public bool trig_active;

    protected float inputHorizontal;
    protected float inputVertical;
    protected bool grounded;
    protected Rigidbody2D rigidBody2D;

    public virtual void Start() {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Update() {
        GetInput();
    }

    public virtual void FixedUpdate() {
        if (BoyClass.boyIsScared == true) {
            MoveHorizontally(ScaredSpeed);
        } else {
            MoveHorizontally(NormalSpeed);
        }
    }

    private void GetInput() {
        if (connectedController != null) {
            inputHorizontal = (Input.GetAxis(connectedController.GetHorizontal()));
            inputVertical = (Input.GetAxis(connectedController.GetVertical()));

            trig_active = connectedController.Trig_CheckInput();
            a_active = connectedController.A_CheckInput();
            b_active = connectedController.B_CheckInput();
            x_active = connectedController.X_CheckInput();
            y_active = connectedController.Y_CheckInput();

            if (a_active && grounded) {
                Jump();
            }
        } else
          {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) && grounded) {
                Jump();
            }
        }
    }

    private void Jump() {
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, JumpForce);
    }

    private void MoveHorizontally(float speed) {
        rigidBody2D.velocity = new Vector2(inputHorizontal * speed, rigidBody2D.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Ground") {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Ground") {
            grounded = false;
        }
    }
}
