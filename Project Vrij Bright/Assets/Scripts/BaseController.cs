using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public ControllerInput connectedController;

    public bool a_active;
    public bool b_active;
    public bool x_active;
    public bool y_active;
    public bool trig_active;

    public float speed = 8;
    public float jumpForce = 10;

    protected float inputHorizontal;
    protected float inputVertical;
    protected bool grounded;

    protected Rigidbody2D rigidBody2D;

    public virtual void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        GetInput();
    }

    public virtual void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(inputHorizontal * speed, rigidBody2D.velocity.y);
    }

    private void GetInput()
    {
       // Debug.Log(connectedController);
        if (connectedController != null) // Check if Joystick exists
        {
            inputHorizontal = (Input.GetAxis(connectedController.GetHorizontal()));
            inputVertical = (Input.GetAxis(connectedController.GetVertical()));

            trig_active = connectedController.Trig_CheckInput();
            a_active = connectedController.A_CheckInput();
            b_active = connectedController.B_CheckInput();
            x_active = connectedController.X_CheckInput();
            y_active = connectedController.Y_CheckInput();
            
            if (a_active && grounded)
            {
                Jump();
            }
        }
        else // else use keyboard input
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");
            
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = false;
        }
    }
}
