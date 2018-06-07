using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol : EnemyBaseClass {
    enum PatrolStates {
        IDLE,
        ALERT,
        CHASE,
        ATTACK,
        DEAD
    }

    public float patrollingDistance = 25;

    private PatrolStates state;

    private float startTime;
    private Vector3 currentPosition;
    private Vector3 newPosition;
    private bool walkRight;
    private bool isPatrolling;

    private new void Start() {
        base.Start();

        state = PatrolStates.IDLE;
        InitPatrolVariables();
    }

    private void InitPatrolVariables() {
        startTime = Time.time;
        currentPosition = transform.position;
        newPosition = new Vector3(transform.position.x + patrollingDistance, transform.position.y, transform.position.z);
        walkRight = true;
    }

    private new void Update() {
        base.Update();

        StateMachine(state);
    }
    
    public override void FindPlayer() {
        base.FindPlayer();
        
        if (playerObject == null) {
            state = PatrolStates.IDLE;
            Debug.Log("No player object!");
            return;
        }

        float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
        Debug.Log(state);
        if (_distanceToPlayer < attackRadius) {
            state = PatrolStates.ATTACK;
        } else if (_distanceToPlayer < chaseRadius) {
            state = PatrolStates.CHASE;
        } else if (_distanceToPlayer < alertRadius) {
            state = PatrolStates.ALERT;
        } else {
            state = PatrolStates.IDLE;
        }
    }

    public override void CheckHealth() {
        if (enemyHealth <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void StateMachine(PatrolStates _state) {
        switch (_state) {
            case PatrolStates.IDLE:
                Patrol();
                break;

            case PatrolStates.ALERT:
                Alert();
                isPatrolling = false;
                break;

            case PatrolStates.CHASE:
                ChaseTarget(playerObject.transform);
                isPatrolling = false;
                break;

            case PatrolStates.ATTACK:
                Attack();
                isPatrolling = false;
                break;

            case PatrolStates.DEAD:
                break;

            default:
                break;
        }
    }

    private void Patrol() {
        if (isPatrolling) {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / patrollingDistance;
            transform.position = Vector2.Lerp(currentPosition, newPosition, fracJourney);

            if (transform.position == newPosition && walkRight) {
                startTime = Time.time;
                walkRight = false;
                currentPosition = transform.position;
                newPosition = new Vector2(transform.position.x - patrollingDistance, transform.position.y);
            } else if (transform.position == newPosition && !walkRight) {
                startTime = Time.time;
                walkRight = true;
                currentPosition = transform.position;
                newPosition = new Vector2(transform.position.x + patrollingDistance, transform.position.y);
            }
        }
        else {
            InitPatrolVariables();
            isPatrolling = true;
        }
    }
    
    private void Alert() {
        float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
        float _temp = _distanceToPlayer;

        if (_distanceToPlayer > _temp) {
            state = PatrolStates.IDLE;
        } else if (_distanceToPlayer < _temp) {
            state = PatrolStates.CHASE;
        }
    }

    private void ChaseTarget(Transform _target) {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, chaseSpeed * Time.deltaTime);
    }

    private void Attack() {
        playerObject.GetComponent<BoyClass>().health -= 1;
    }
}
