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

    private bool teleported = false;
    private bool isContent = false;

    // FMOD
    [FMODUnity.EventRef]
    public string alertEvent;
    private bool playedAlertAudio = false;
    [FMODUnity.EventRef]
    public string attackEvent;
    [FMODUnity.EventRef]
    public string deathEvent;
    [FMODUnity.EventRef]
    public string idleEvent;
    private FMOD.Studio.EventInstance idleInstance;

    new private void Start()
    {
        base.Start();
        currentState = EnemyStates.IDLE;
        anim = GetComponentInChildren<Animator>();

        // FMOD
        idleInstance = FMODUnity.RuntimeManager.CreateInstance(idleEvent);
    }

    //does not call base update because of statemachine
    new private void Update()
    {
        FindBait();
        StateMachine(currentState);
        SetLight();

        idleInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
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
                break;

            case EnemyStates.IDLE:
                idleInstance.start();
                break;

            case EnemyStates.CHASEBAIT:
                targetTransform = bait.transform;
                EnemyMovement();
                if (!playedAlertAudio)
                {
                    idleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    FMODUnity.RuntimeManager.PlayOneShotAttached(alertEvent, gameObject);
                    playedAlertAudio = true;
                }
                break;

            case EnemyStates.ALERT:
                if (!playedAlertAudio)
                {
                    idleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    FMODUnity.RuntimeManager.PlayOneShotAttached(alertEvent, gameObject);
                    playedAlertAudio = true;
                }
                break;

            case EnemyStates.CHASE:
                targetTransform = playerObject.transform;
                EnemyMovement();
                break;

            case EnemyStates.ATTACK:
                targetTransform = null;
                Attack();
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

    public override void Attack()
    {
        if (Time.time > nextAttack)
        {
            playerObject.GetComponent<BoyClass>().health -= 8;
            nextAttack = Time.time + attackRate;
            FMODUnity.RuntimeManager.PlayOneShotAttached(attackEvent, this.gameObject);
        }
    }
}
