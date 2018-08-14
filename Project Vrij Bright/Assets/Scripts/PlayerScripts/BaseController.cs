using UnityEngine;

public class BaseController : MonoBehaviour
{
    public ControllerInput connectedController;
    public SpriteRenderer spriteRenderer;
    public bool a_active;
    public bool b_active;
    public bool x_active;
    public bool y_active;
    public bool trig_active;

    protected float inputHorizontal;
    protected float inputVertical;
    protected float walkSpeed;
    [SerializeField]
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

    public virtual void Jump(float force)
    {
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, force);
    }

    protected void MoveHorizontally(float speed)
    {
        rigidBody2D.velocity = new Vector2(inputHorizontal * speed, rigidBody2D.velocity.y);
        walkSpeed = rigidBody2D.velocity.x;
    }

    public GameObject RayCaster(Vector2 _position, Vector2 _direction, float _distance)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_position, _direction, _distance);
        if (_hit)
        {
            return _hit.transform.gameObject;
        }
        return null;
    }

    public virtual void GetInput()
    {
        if (connectedController != null)
        {
            inputHorizontal = (Input.GetAxisRaw(connectedController.GetHorizontal()));
            inputVertical = (Input.GetAxisRaw(connectedController.GetVertical()));

            trig_active = connectedController.Trig_CheckInput();
            a_active = connectedController.A_CheckInput();
            b_active = connectedController.B_CheckInput();
            x_active = connectedController.X_CheckInput();
            y_active = connectedController.Y_CheckInput();

            if (inputHorizontal != 0)
                FlipSprite(connectedController.GetHorizontal());
        }
        else
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");

            FlipSprite("Horizontal");
        }
    }

    public void FlipSprite(string _input)
    {
        float _direction = Mathf.Sign(Input.GetAxis(_input));
        if (_direction == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (_direction == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = false;
        }
    }

}
