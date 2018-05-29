using UnityEngine;
using System.Collections;
public class BoyController : BaseController
{

    //basevalues for resetting physics
    public static float NormalSpeed = 8;
    public static float normalJump = 5;

    public float currentSpeed = 8;
    public float currentJump = 5;

    //animator settings
    public Animator boyAnimator;
    private bool Scared = false;
    private bool Attacking = false;
    private bool Walking = false;
    private bool Jumping = false;

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
        if (walkSpeed != 0){
            SetAnimatorBool("Walking", true);
        }else{
            SetAnimatorBool("Walking", false);
        }
    }

    public override void GetInput(){
        base.GetInput();

        if (connectedController != null){
            if (a_active && grounded){
                Jump(normalJump);
                StartCoroutine(PlayAnim("Jumping"));
            }

            if (x_active){
                StartCoroutine(PlayAnim("Attacking"));
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.Space) && grounded){
                Jump(normalJump);
                StartCoroutine(PlayAnim("Jumping"));
            }

            if (Input.GetKeyDown(KeyCode.E)){
                StartCoroutine(PlayAnim("Attacking"));
            }
        }
    }

    //override collision check so players can jump on eachothers head
    new public void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Guardian"){
            grounded = true;
            SetAnimatorBool("Jumping", false);
        }
    }


    //override collision check so players can jump on eachothers head
    new public void OnCollisionExit2D(Collision2D collision){
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Guardian"){
            grounded = false;
        }
    }


    //sets all active false for a brief moment to reset velocity and physics
    public void SetAllInputFalse(){
        a_active = false;
        b_active = false;
        x_active = false;
        y_active = false;
        trig_active = false;
    }

    private void BasicAttack(Collider2D collision){
        EnemyBaseClass _enemyScript = collision.GetComponent<EnemyBaseClass>();
        int _damage = GetComponent<BoyClass>().attackDamage;
        _enemyScript.TakeDamage(_damage);
    }


    //Change player properties on trigger enter
    private void OnTriggerEnter2D(Collider2D collision){
        //behaviour for when player is scared
        if (collision.tag == "Shadow" &&  this.gameObject.layer != 14){
            SetAnimatorBool("Scared", true);
            SetAllInputFalse();
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed, 0.25f);
        }


        else if (collision.tag == "GravityWell" && this.gameObject.layer != 14)
        {
            SetAllInputFalse();
            PhysicsScript.GravityIncrease(this.gameObject, 0.5f, 2f);
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed, 0.45f);
        }

        //makes player visible when entering mirror in mirrorworld
        else if (collision.tag == "Mirror" && this.gameObject.layer == 14){
            sprR.enabled = true;
        }
    }

    //Restore player properties on trigger exit 
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.transform.tag == "Shadow" ){
            SetAnimatorBool("Scared", false);
            SetAllInputFalse();
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed);
        }

        else if (collision.tag == "GravityWell"){
            SetAllInputFalse();
            PhysicsScript.ResetGravity(this.gameObject);
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed);
        }

        //makes player invisible when leaving in mirrorworld
        else if (collision.tag == "Mirror" && this.gameObject.layer == 14){
            sprR.enabled = false;
        }
    }

    //set player to mirror layer
    //when in mirror layer, Physics2D doesn't detect collision with objects that are layered "Obstacle"
    //also makes you invicible when leaving mirror in mirror world to simulate mirror world effect.
    //layer 14 = mirror layer/ layer 15 = player layer
    private void EnterOrLeaveMirrorWorld(){
        if (this.gameObject.layer == 14){
            this.gameObject.layer = 15;
        }

        else if (this.gameObject.layer != 14){
            this.gameObject.layer = 14;
        }
    }

    private void SetAnimatorBool(string _boolName, bool _bool){
        boyAnimator.SetBool(_boolName, _bool);
    }

    private IEnumerator PlayAnim(string _boolname){
        SetAnimatorBool(_boolname, true);
        yield return new WaitForSeconds(0.5f);
        SetAnimatorBool(_boolname, false);
    }


    private void OnTriggerStay2D(Collider2D collision){
        if (collision.tag == "Monster" && x_active || collision.tag == "Monster" && Input.GetKeyDown(KeyCode.E)){
            Debug.Log("attack");
            BasicAttack(collision);
            //SetAnimatorBool("Attacking", false);
        }

        else if (collision.tag == "Mirror" && x_active || collision.tag == "Mirror" && Input.GetKeyDown(KeyCode.E)){
            Debug.Log("In mirror");
            EnterOrLeaveMirrorWorld();

        }
    }
}

