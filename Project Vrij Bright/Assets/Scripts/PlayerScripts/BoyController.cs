using UnityEngine;
using System.Collections;

public class BoyController : BaseController {

    public Transform raycastPos;
    public Transform raycastpos;

    //basevalues for resetting physics
    public static float normalSpeed = 0.5f;
    public static float normalJump = 6;
    public float currentSpeed = 0.5f;
    public float currentJump = 5;
    //private string[] jumpableLayers;

    public float jumpHeight = 4;
    public float jumpVelocity;
    public float timeToJumpApex = .4f;
    public float gravity = 4f;

	// FMOD
	[FMODUnity.EventRef]
	public string eventRef;
	private FMOD.Studio.EventInstance instance;
	private bool isPlayingFootsteps = false;

    private Vector3 moveDirection;

    //animator settings
    public Animator boyAnimator;
    public bool coolingDown = false;
    private bool scared = false;
    private bool attacking = false;
    private bool walking = false;
    private bool jumping = false;
    
    private float attackRate = 0.8f;
    private float nextAttack;

    public override void Start() {
        base.Start();

        gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2) * 0.2f;
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        moveDirection.y = -1;

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0) {
            connectedController = new Joystick1();
        }
		instance = FMODUnity.RuntimeManager.CreateInstance (eventRef);
    }

    public override void Update() {
        base.Update();
        if (moveDirection.y != 0) {
            transform.Translate(JumpDir() * Time.deltaTime);
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        MoveHorizontally(currentSpeed);
        if (walkSpeed != 0) {
			if (!isPlayingFootsteps) {
				instance.start ();
				isPlayingFootsteps = true;
			}
            SetAnimatorBool("Walking", true);

        } else {
            SetAnimatorBool("Walking", false);
			instance.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			isPlayingFootsteps = false;
        }
    }

    private Vector3 JumpDir() {
        if (IsGrounded()) {
            moveDirection = new Vector3(0, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= 1.5f;

            if (b_active) {
                moveDirection.y = jumpHeight;
            }
        }

        if (!IsGrounded()) {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        return moveDirection;
        //transform.Translate(moveDirection * Time.deltaTime);
    }

    private bool IsGrounded() {
        GameObject hitObject = RayCaster2D(raycastpos.position, Vector2.down, 0.05f);
        if (hitObject != null) {
           if  (hitObject.tag == "Ground") {
                Debug.Log("Boy is grounded.");
                return true;
            }
        }
        Debug.Log("Boy is not grounded.");
        return false;
    }

    private GameObject RayCaster2D(Vector2 _origin, Vector2 _dir, float _dist) {
        RaycastHit2D hit = Physics2D.Raycast(_origin, _dir, _dist);
        if (hit) {
            return hit.transform.gameObject;
        }
        return null;
    }
    
    public override void GetInput() {
        base.GetInput();

        if (connectedController != null) {
            if (a_active && IsGrounded()) {
                //Jump(normalJump);
                StartCoroutine(PlayAnim("Jumping"));
            }
            if (x_active && Time.time > nextAttack) {
                StartCoroutine(PlayAnim("Attacking"));
                BasicAttack();
                nextAttack = Time.time + attackRate;
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
                Jump(normalJump);
                StartCoroutine(PlayAnim("Jumping"));
            }
            if (Input.GetKeyDown(KeyCode.E) && Time.time > nextAttack) {
                StartCoroutine(PlayAnim("Attacking"));
                BasicAttack();
                nextAttack = Time.time + attackRate;
                Debug.Log("attack!");
            }
        }
    }

    //sets all active false for a brief moment to reset velocity and physics
    public void SetAllInputFalse() {
        a_active = false;
        b_active = false;
        x_active = false;
        y_active = false;
        trig_active = false;
    }

    private void BasicAttack() {
        GameObject attackObject = RayCaster(raycastPos.transform.position, Vector2.right, 0.2f);
        if (attackObject != null) {
            if (attackObject.tag == "Monster") {
                EnemyBaseClass _enemyScript = attackObject.GetComponent<EnemyBaseClass>();
                int _damage = GetComponent<BoyClass>().attackDamage;
                _enemyScript.TakeDamage(_damage);
            }
        }
    }

    //Change player properties on trigger enter
    private void OnTriggerEnter2D(Collider2D collision) {
        //behaviour for when player is scared
        if (collision.tag == "Shadow" && this.gameObject.layer != 14) {
            SetAnimatorBool("Scared", true);
            SetAllInputFalse();
            currentSpeed = PhysicsScript.EffectedFloat(normalSpeed, 0.25f);
        } else if (collision.tag == "GravityWell" && this.gameObject.layer != 14) {
            SetAllInputFalse();
            PhysicsScript.GravityIncrease(this.gameObject, 0.5f, 1.5f);
            currentSpeed = PhysicsScript.EffectedFloat(normalSpeed, 0.45f);
        }

          //makes player visible when entering mirror in mirrorworld
          else if (collision.tag == "Mirror" && this.gameObject.layer == 14) {
            sprR.enabled = true;
        }
    }

    //Restore player properties on trigger exit 
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.tag == "Shadow") {
            SetAnimatorBool("Scared", false);
            SetAllInputFalse();
            currentSpeed = PhysicsScript.EffectedFloat(normalSpeed);
        } else if (collision.tag == "GravityWell") {
            SetAllInputFalse();
            PhysicsScript.ResetGravity(this.gameObject);
            currentSpeed = PhysicsScript.EffectedFloat(normalSpeed);
        }

          //makes player invisible when leaving in mirrorworld
          else if (collision.tag == "Mirror" && this.gameObject.layer == 14) {
            sprR.enabled = false;
        }
    }

    private void SetAnimatorBool(string _boolName, bool _bool) {
        boyAnimator.SetBool(_boolName, _bool);
    }

    //play animation and set bool to false again
    private IEnumerator PlayAnim(string _boolname) {
        SetAnimatorBool(_boolname, true);
        yield return new WaitForSeconds(0.5f);
        SetAnimatorBool(_boolname, false);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Mirror") {
            if (b_active) {
                collision.gameObject.GetComponent<Interaction>().Teleport(this.gameObject);
            } else {
                collision.gameObject.GetComponent<Interaction>().SetButtonActive(true);
            }
        }
    }
}

