using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol : EnemyBaseClass {

    public float patrollingDistance = 25;

    private float startTime;
    private Vector3 currentPosition;
    private Vector3 newPosition;
    private bool walkRight;
    private bool isPatrolling;

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.ParameterInstance monsterStatus;
    private float idle = 0;
    private float alert = 20;
    private float attack = 30;
    private float dead = 50;

    private new void Start() {
        base.Start();

        currentState = EnemyStates.IDLE;
        InitPatrolVariables();
        Debug.Log(playerObject);

        // FMOD
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, this.gameObject.transform, rigidBody2D);
        instance.start(); ;
        instance.getParameter("M1 Status", out monsterStatus);
    }

    private void InitPatrolVariables() {
        startTime = Time.time;
        currentPosition = transform.position;
        newPosition = new Vector3(transform.position.x + patrollingDistance, transform.position.y, transform.position.z);
        walkRight = true;
    }

    private new void Update() {
        base.Update();

        StateMachine(currentState);
        instance.getParameter("M1 Status", out monsterStatus);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
    }

    public override void CheckHealth() {
        if (enemyHealth <= 0) {
            Enemy_Respawn respawnScript = GetComponent<Enemy_Respawn>();
            if (respawnScript) {
                respawnScript.Die();
            } else {
                Destroy(this.gameObject);
                currentState = EnemyStates.DEAD;
            }
        }
    }

    private void StateMachine(EnemyStates _state) {
        switch (_state) {
            case EnemyStates.IDLE:
                monsterStatus.setValue(idle);
                Patrol();
                break;

            case EnemyStates.ALERT:
                Alert();
                isPatrolling = false;
                monsterStatus.setValue(alert);
                break;

            case EnemyStates.CHASE:
                ChaseTarget(playerObject.transform);
                isPatrolling = false;
                break;

            case EnemyStates.ATTACK:
                Attack();
                isPatrolling = false;
                monsterStatus.setValue(attack);
                break;

            case EnemyStates.DEAD:
                monsterStatus.setValue(dead);
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
        } else {
            InitPatrolVariables();
            isPatrolling = true;
        }
    }

    private void Alert() {
        float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
        float _temp = _distanceToPlayer;

        if (_distanceToPlayer > _temp) {
            currentState = EnemyStates.IDLE;
        } else if (_distanceToPlayer < _temp) {
            currentState = EnemyStates.CHASE;
        }
    }
}
