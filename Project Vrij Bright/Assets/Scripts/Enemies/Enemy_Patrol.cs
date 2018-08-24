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

    private new void Start()
    {
        base.Start();

        currentState = EnemyStates.IDLE;
        InitPatrolVariables();
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
    }

    public override void CheckHealth()
    {
        if (enemyHealth <= 0)
        {
            Destroy(this.gameObject);
            currentState = EnemyStates.DEAD;
        }
    }

    private void StateMachine(EnemyStates _state)
    {
        switch (_state)
        {
            case EnemyStates.IDLE:
                Patrol();
                break;

            case EnemyStates.ALERT:
                Alert();
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

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        yield return null;
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
}
