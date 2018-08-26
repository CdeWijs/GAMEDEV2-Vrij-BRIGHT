using UnityEngine;
using System.Collections;

public class BoyController : BaseController
{
    public LayerMask attackLayerMask;
    public Transform raycastPos;
    private Vector3 moveDirection;

    //basevalues for resetting physics
    public static float NormalSpeed = 8f;
    public static float normalJump = 6;

    public float currentSpeed = 0.5f;
    public float currentJump = 5;
    //private string[] jumpableLayers;

    public float jumpHeight = 4;
    public float jumpVelocity;
    public float timeToJumpApex = .4f;
    public float gravity = 4f;

    //animator settings
    public Animator boyAnimator;

    private float attackRate = 0.8f;
    private float nextAttack;

    float[] raycastPoints = new float[3];
    public Vector2[] raycastLocations = new Vector2[3];
    public Transform raycastpos;

    // FMOD
    [FMODUnity.EventRef]
    public string footStepEvent;
    private FMOD.Studio.EventInstance footStepInstance;
    private bool isPlayingFootsteps = false;
    [FMODUnity.EventRef]
    public string attackEvent;

    public override void Start()
    {
        base.Start();
        CalculateRayCastPoints();
        gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2) * 0.2f;
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0)
        {
            if (!switchControls)
            {
                connectedController = new Joystick1();
            }
            else
            {
                connectedController = new Joystick2();
            }
        }

        footStepInstance = FMODUnity.RuntimeManager.CreateInstance(footStepEvent);
    }

    public override void Update()
    {
        base.Update();
        //transform.Translate(JumpDir() * Time.deltaTime);
    }

    public void FixedUpdate()
    {
        MoveHorizontally(currentSpeed);
        //set animations if player is walking
        if (walkSpeed != 0)
        {
            if (!isPlayingFootsteps && Grounded())
            {
                footStepInstance.start();
                isPlayingFootsteps = true;
            }
            SetAnimatorBool("Walking", true);
        }
        else
        {
            footStepInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlayingFootsteps = false;
            SetAnimatorBool("Walking", false);
        }
    }


    public override void GetInput()
    {
        base.GetInput();

        if (connectedController != null)
        {
            if (a_active && grounded)
            {
                Jump(normalJump);
                StartCoroutine(PlayAnim("Jumping"));
            }
            if (x_active && Time.time > nextAttack)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached(attackEvent, this.gameObject);
                StartCoroutine(PlayAnim("Attacking"));
                //    BasicAttack();
                nextAttack = Time.time + attackRate;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && Grounded())
            {
                Jump(normalJump);
                StartCoroutine(PlayAnim("Jumping"));
            }
            if (Input.GetKeyDown(KeyCode.E) && Time.time > nextAttack)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached(attackEvent, this.gameObject);
                StartCoroutine(PlayAnim("Attacking"));
                //BasicAttack();
                nextAttack = Time.time + attackRate;
            }
        }
    }

    //sets all active false for a brief moment to reset velocity and physics
    public void SetAllInputFalse()
    {
        a_active = false;
        b_active = false;
        x_active = false;
        y_active = false;
        trig_active = false;
    }

    private void BasicAttack(GameObject attackObject)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 8.0f, attackLayerMask))
        {
            EnemyBaseClass enemyScript = hit.transform.GetComponent<EnemyBaseClass>();
            Debug.Log(hit.transform.gameObject);
            enemyScript.TakeDamage(GetComponent<BoyClass>().attackDamage);
        }
        //GameObject attackObject = RayCaster(raycastPos.transform.position, Vector2.right, 8f);
        Debug.Log(attackObject);
        if (attackObject != null)
        {
            if (attackObject.tag == "Monster")
            {
                EnemyBaseClass _enemyScript = attackObject.GetComponent<EnemyBaseClass>();
                int _damage = GetComponent<BoyClass>().attackDamage;
                _enemyScript.TakeDamage(_damage);
            }
        }
    }

    private void SetAnimatorBool(string _boolName, bool _bool)
    {
        boyAnimator.SetBool(_boolName, _bool);
    }

    //play animation and set bool to false again
    private IEnumerator PlayAnim(string _boolname)
    {
        SetAnimatorBool(_boolname, true);
        yield return new WaitForSeconds(0.5f);
        SetAnimatorBool(_boolname, false);
    }

    #region Triggers

    //Change player properties on trigger enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //behaviour for when player is scared
        if (collision.tag == "Shadow" && this.gameObject.layer != 14)
        {
            SetAnimatorBool("Scared", true);
            SetAllInputFalse();
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed, 0.5f);
        }
        else if (collision.tag == "GravityWell" && this.gameObject.layer != 14)
        {
            SetAllInputFalse();
            PhysicsScript.GravityIncrease(this.gameObject, 0.5f, 1.5f);
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed, 0.45f);
        }
        //makes player visible when entering mirror in mirrorworld
        else if (collision.tag == "Mirror" && this.gameObject.layer == 14)
        {
            spriteRenderer.enabled = true;
        }
    }

    //Restore player properties on trigger exit 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Shadow")
        {
            Debug.Log("Leaving shadows");
            SetAnimatorBool("Scared", false);
            SetAllInputFalse();
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed, 1);
        }
        else if (collision.tag == "GravityWell")
        {
            SetAllInputFalse();
            PhysicsScript.ResetGravity(this.gameObject, new Vector3(1, 1, 1));
            currentSpeed = PhysicsScript.EffectedFloat(NormalSpeed);
        }
        //makes player invisible when leaving in mirrorworld
        else if (collision.tag == "Mirror" && this.gameObject.layer == 14)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerMirror(collision);
        OnTriggerRope(collision);
        OnTriggerMonster(collision);
    }

    private void OnTriggerMirror(Collider2D collision)
    {
        if (collision.tag == "Mirror")
        {

            if (connectedController != null)
            {
                if (b_active)
                {
                    collision.gameObject.GetComponent<Interaction>().Teleport(this.gameObject);
                    Jump(normalJump);
                    StartCoroutine(PlayAnim("Jumping"));
                }
                else
                {
                    collision.gameObject.GetComponent<Interaction>().SetButtonActive(true);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    collision.gameObject.GetComponent<Interaction>().Teleport(this.gameObject);
                    Jump(normalJump);
                    StartCoroutine(PlayAnim("Jumping"));
                }
                else
                {
                    collision.gameObject.GetComponent<Interaction>().SetButtonActive(true);
                }
            }
        }
    }

    private void OnTriggerRope(Collider2D collision)
    {
        if (collision.tag == "Rope")
        {
            if (connectedController != null && x_active)
            {
                collision.gameObject.GetComponent<CageScript>().DropCage();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                collision.gameObject.GetComponent<CageScript>().DropCage();
            }
        }
    }

    private void OnTriggerMonster(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            if (connectedController != null)
            {
                if (x_active && Time.time > nextAttack)
                {
                    StartCoroutine(PlayAnim("Attacking"));
                    BasicAttack(collision.gameObject);
                    nextAttack = Time.time + attackRate;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E) && Time.time > nextAttack)
                {
                    StartCoroutine(PlayAnim("Attacking"));
                    BasicAttack(collision.gameObject);
                    nextAttack = Time.time + attackRate;
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// Jumping functionality:
    /// 
    /// Void -> CalculateRaypoints():   Calculates the spacing between the raycasts by deviding the width of the player by 3 and sets a vector2 array where index[0] is the x value of the raycastspawn object.
    /// GO   -> Raycaster2D():          Takes in the raycast vector2 array and loops through the coordinates in it. It then returns the gameobject the player is colliding with and ends the loop
    /// Bool -> Grounded():             checks is RaycasterToday object is ground/obstacle object
    /// v3   -> JumpDir():              This is the direction the player is going. when jump_button is pressed -> player is translated with a jumpvelocity -> gravity is then substracted from jumpvelocity to move player down. gravity increases by time
    /// 
    /// </summary>
    /// 

    private Vector3 JumpDir()
    {
        if (Grounded())
        {
            moveDirection = new Vector3(0, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= 1.5f;

            if (a_active)
            {
                // moveDirection.y = jumpHeight;
            }
        }

        if (!Grounded())
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        return moveDirection;
    }

    private bool Grounded()
    {
        CalculateRayCastPoints();
        GameObject hitObject = RayCaster2D(raycastLocations, Vector2.down, 0.025f);
        if (hitObject != null)
        {
            //layer 13 = obstacle layer;
            if (hitObject.tag == "Ground" || hitObject.layer == 13)
            {
                return true;
            }
        }
        return false;
    }

    private void CalculateRayCastPoints()
    {
        BoxCollider2D bc2d = this.gameObject.GetComponent<BoxCollider2D>();
        float sizeDivided = 0;
        raycastLocations[0] = raycastpos.transform.position;

        for (int i = 1; i < raycastPoints.Length; i++)
        {
            raycastPoints[i] = sizeDivided + bc2d.size.x / 3;
            raycastLocations[i] = new Vector2(raycastPoints[i] + raycastPos.transform.position.x, raycastPos.transform.position.y);
        }
    }

    private GameObject RayCaster2D(Vector2[] _origin, Vector2 _dir, float _dist)
    {
        RaycastHit2D[] hit = new RaycastHit2D[3];
        for (int i = 0; i < hit.Length; i++)
        {
            hit[i] = Physics2D.Raycast(_origin[i], _dir, _dist);
            Debug.DrawRay(_origin[i], _dir, Color.red, 0.1f);

        }
        for (int j = 0; j < hit.Length; j++)
        {
            if (hit[j])
            {
                return hit[j].transform.gameObject;
            }
        }
        return null;
    }
}

