using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : BaseController
{
    private float startGravityScale;

    public override void Start()
    {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0)
        {
            connectedController = new Joystick2();
        }

        startGravityScale = rigidBody2D.gravityScale;
    }

    public override void Update()
    {
        base.Update();

        if (connectedController != null)
        {
            if (trig_active)
            {
                rigidBody2D.gravityScale = 0;
                transform.position = new Vector2(transform.position.x, transform.position.y + 3f * Time.deltaTime);
            }
            else if (rigidBody2D.gravityScale == 0)
            {
                rigidBody2D.gravityScale = startGravityScale;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody2D.gravityScale = 0;
                transform.position = new Vector2(transform.position.x, transform.position.y + 3f * Time.deltaTime);
            }
            else if (rigidBody2D.gravityScale == 0)
            {
                rigidBody2D.gravityScale = startGravityScale;
            }
        }
    }
}
