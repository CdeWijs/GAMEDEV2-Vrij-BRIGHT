using System.Collections;
using UnityEngine;

public class Enemy_Patrol : EnemyBaseClass
{
    public float patrollingDistance = 25;

    private float startTime;
    private Vector3 currentPosition;
    private Vector3 newPosition;
    private bool walkRight;
    private bool isPatrolling;
    private float startY;

    // FMOD
    [FMODUnity.EventRef]
    public string alertEvent;
    private bool playedAlertAudio = false;
    [FMODUnity.EventRef]
    public string attackEvent;
    [FMODUnity.EventRef]
    public string deathEvent;
    private bool playedDeathAudio = false;
    [FMODUnity.EventRef]
    public string idleEvent;
    private FMOD.Studio.EventInstance idleInstance;

    private new void Start()
    {
        base.Start();

        currentState = EnemyStates.IDLE;
        InitPatrolVariables();
        startY = transform.position.y;

        idleInstance = FMODUnity.RuntimeManager.CreateInstance(idleEvent);
    }

    private void InitPatrolVariables()
    {
        startTime = Time.time;
        currentPosition = transform.position;
        newPosition = new Vector3(transform.position.x + patrollingDistance, transform.position.y, transform.position.z);
        walkRight = true;
    }

    private new void Update()
    {
        base.Update();
        StateMachine(currentState);
        idleInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
        if (transform.position.y < -5)
        {
            transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        }
    }

    public override void CheckHealth()
    {
        if (enemyHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        if (!playedDeathAudio)
        {
            idleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODUnity.RuntimeManager.PlayOneShotAttached(deathEvent, gameObject);
            playedDeathAudio = true;
        }
        currentState = EnemyStates.DEAD;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    private void StateMachine(EnemyStates _state)
    {
        switch (_state)
        {
            case EnemyStates.IDLE:
                idleInstance.start();
                Patrol();
                break;

            case EnemyStates.ALERT:
                Alert();
                if (!playedAlertAudio)
                {
                    idleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    FMODUnity.RuntimeManager.PlayOneShotAttached(alertEvent, this.gameObject);
                    playedAlertAudio = true;
                }
                isPatrolling = false;
                break;

            case EnemyStates.CHASE:
                ChaseTarget(playerObject.transform);
                isPatrolling = false;
                break;

            case EnemyStates.ATTACK:
                Attack();
                isPatrolling = false;
                break;

            case EnemyStates.DEAD:
                findPlayer = false;
                StartCoroutine(Die());
                break;

            default:
                break;
        }
    }

    private void Patrol()
    {
        if (isPatrolling)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / patrollingDistance;
            transform.position = Vector2.Lerp(currentPosition, newPosition, fracJourney);

            if (transform.position == newPosition && walkRight)
            {
                startTime = Time.time;
                walkRight = false;
                currentPosition = transform.position;
                newPosition = new Vector2(transform.position.x - patrollingDistance, transform.position.y);
            }
            else if (transform.position == newPosition && !walkRight)
            {
                startTime = Time.time;
                walkRight = true;
                currentPosition = transform.position;
                newPosition = new Vector2(transform.position.x + patrollingDistance, transform.position.y);
            }
        }
        else
        {
            InitPatrolVariables();
            isPatrolling = true;
        }
    }

    private void Alert()
    {
        float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
        float _temp = _distanceToPlayer;

        if (_distanceToPlayer > _temp)
        {
            currentState = EnemyStates.IDLE;
        }
        else if (_distanceToPlayer < _temp)
        {
            currentState = EnemyStates.CHASE;
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
