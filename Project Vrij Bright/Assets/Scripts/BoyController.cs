using UnityEngine;
public class BoyController : BaseController
{

    //basevalues for resetting physics
    public static float NormalSpeed = 8;
    public static float normalJump = 5;

    public float currentSpeed = 8;
    public float currentJump = 5;

    public override void Start(){
        base.Start();

        
        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0){
            connectedController = new Joystick1();
        }
    }

    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();

        MoveHorizontally(currentSpeed);
    }

    public override void GetInput(){
        base.GetInput();

        if (connectedController != null){
            if (a_active && grounded){
                Jump(normalJump);
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.Space) && grounded){
                Jump(normalJump);
            }
        }
    }

    private void BasicAttack(Collider2D collision){
        EnemyBaseClass _enemyScript = collision.GetComponent<EnemyBaseClass>();
        int _damage = GetComponent<BoyClass>().attackDamage;
        _enemyScript.TakeDamage(_damage);
    }


    //Change player properties on trigger enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //behaviour for when player is scared
        if (collision.tag == "Shadow"){
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed , 0.25f);
        }


        else if (collision.tag == "GravityWell"){
            PhysicsScript.GravityIncrease(this.gameObject, 0.5f, 2f);
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed, 0.45f);
        }
    }

    //Restore player properties on trigger exit 
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.transform.tag == "Shadow"){
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed);
        }

        else if (collision.tag == "GravityWell"){
            PhysicsScript.ResetGravity(this.gameObject);
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed);
        }
    }


    private void OnTriggerStay2D(Collider2D collision){
        if (x_active || Input.GetKeyDown(KeyCode.E)){
            if (collision.tag == "Monster"){
                Debug.Log("attack");
                BasicAttack(collision);
            }
        }
    }
}
