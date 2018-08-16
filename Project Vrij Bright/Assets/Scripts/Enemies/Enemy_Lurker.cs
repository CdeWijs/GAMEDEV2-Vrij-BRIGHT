using UnityEngine;

/// <summary>
/// class for the lurker type enemy: enemy is invincible while in shadows, but can be attacked when it comes out of hiding by using bait
/// </summary>
public class Enemy_Lurker : EnemyBaseClass
{
    public GameObject bait;
    public bool isInShadows = true;
    public GameObject lt;
    public bool canTeleport = false;

    private Transform targetTransform;
    private float baitRadius = 30f;

    private Animator anim;

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.ParameterInstance monsterStatus;
    private float idle = 0;
    private float alert = 20;
    private float attack = 30;
    private float dead = 50;

    private bool teleported = false;
    private bool isContent = false;

    new private void Start()
    {
        base.Start();
        currentState = EnemyStates.IDLE;
        anim = GetComponentInChildren<Animator>();

        // FMOD
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, this.gameObject.transform, rigidBody2D);
        instance.start(); ;
        instance.getParameter("M1 Status", out monsterStatus);
        monsterStatus.setValue(idle);
    }

    //does not call base update because of statemachine
    new private void Update()
    {
        Debug.Log(currentState);
        FindBait();
        StateMachine(currentState);
        SetLight();

        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
    }

    public void SetLight()
    {
        if (isInShadows)
        {
            // lt.SetActive(true);
        }
        else
        {
            lt.SetActive(false);
        }
    }
    //enemy is invulnerable in shadows
    public override void CheckHealth()
    {
        if (!isInShadows)
        {
            if (enemyHealth <= 0)
            {
                monsterStatus.setValue(dead);
                Destroy(this.gameObject);
            }
        }
    }


    public override void TakeDamage(int amount)
    {
        if (!isInShadows)
        {
            base.TakeDamage(amount);
        }
    }
    //enemy moves towards target 
    public override void EnemyMovement()
    {
        Vector3 moveToPos = new Vector3(targetTransform.transform.position.x, transform.position.y, 0);
        transform.position = Vector2.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
    }

    //checks if bait is on the ground and if enemy should come out of hiding
    private void FindBait()
    {
        if (bait.gameObject == null)
        {
            return;
        }

        if (bait.transform.position.y < 1.0f)
        {
            bait.GetComponent<BaitScript>().baitOnGround = true;
        }

        float _distanceToBait = Mathf.Abs((bait.transform.position.x - transform.position.x));
        if (Mathf.RoundToInt(bait.transform.position.y) == Mathf.RoundToInt(transform.position.y))
        {
            if (bait.GetComponent<BaitScript>().baitOnGround)
            {
                currentState = EnemyStates.CHASEBAIT;
            }
        }
        else if (_distanceToBait < baitRadius)
        {
            if (bait.GetComponent<BaitScript>().baitOnGround)
            {
                currentState = EnemyStates.CHASEBAIT;
            }
        }
    }

    #region Triggers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bait")
        {
            EatBait(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Shadow")
        {
            isInShadows = true;
        }
        else if (collision.gameObject.tag == "Mirror" && canTeleport)
        {
            if (!teleported)
            {
                collision.gameObject.GetComponent<Interaction>().Teleport(this.gameObject);
                teleported = true;
            }
        }
    }

    //check if enemy is in shadows 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Shadow")
        {
            isInShadows = false;
        }
    }
    #endregion

    //enemy eats bait and returns to idle state
    private void EatBait(GameObject bait)
    {
        chaseRadius = 0;
        bait.GetComponent<BaitScript>().baitOnGround = false;
        isInShadows = false;
        findPlayer = false;
        currentState = EnemyStates.CONTENT;
        transform.position = bait.transform.position;
        Destroy(bait);
    }

    //state machine for lurker enemy
    private void StateMachine(EnemyStates state)
    {
        switch (state)
        {
            case EnemyStates.NULL:
                monsterStatus.setValue(idle);
                break;

            case EnemyStates.IDLE:
                //perhaps Idle animation
                break;

            case EnemyStates.CHASEBAIT:
                targetTransform = bait.transform;
                EnemyMovement();
                monsterStatus.setValue(alert);
                break;

            case EnemyStates.ALERT:
                monsterStatus.setValue(alert);
                break;

            case EnemyStates.CHASE:
                targetTransform = playerObject.transform;
                EnemyMovement();
                break;

            case EnemyStates.ATTACK:
                targetTransform = null;
                Attack();
                monsterStatus.setValue(attack);
                break;

            case EnemyStates.CONTENT:
                if (!isContent)
                {
                    isContent = true;
                    anim.SetBool("isContent", true);
                }
                break;

            default:
                break;
        }
    }
}
