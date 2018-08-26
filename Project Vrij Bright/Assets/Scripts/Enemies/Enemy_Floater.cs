using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Floater : EnemyBaseClass {
    
    public float floatindDistanceX;
    public float floatingDistanceY;

    private float startTimeX;
    private Vector3 currentPositionX;
    private Vector3 newPositionX;
    private bool floatRight;

    private float startTimeY;
    private Vector3 currentPositionY;
    private Vector3 newPositionY;
    private bool floatUp;
    private bool isFloating;

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

    private new void Start() {
        base.Start();
        currentState = EnemyStates.IDLE;
        InitFloatVariables();
        idleInstance = FMODUnity.RuntimeManager.CreateInstance(idleEvent);
    }

    private void InitFloatVariables() {
        startTimeX = Time.time;
        currentPositionX = transform.position;
        newPositionX = new Vector2(transform.position.x + floatindDistanceX, 0);
        floatRight = true;

        startTimeY = Time.time;
        currentPositionY = transform.position;
        newPositionY = new Vector2(0, transform.position.y + floatingDistanceY);
        floatUp = true;
    }

    private new void Update() {
        base.Update();
        StateMachine(currentState);
        idleInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
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

    private void StateMachine(EnemyStates _state) {
        switch (_state) {
            case EnemyStates.IDLE:
                transform.position = FloatHorizontally() + FloatVertically();
                break;

            case EnemyStates.ALERT:
                Alert();
                if (!playedAlertAudio)
                {
                    idleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    FMODUnity.RuntimeManager.PlayOneShotAttached(alertEvent, this.gameObject);
                    playedAlertAudio = true;
                }
                isFloating = false;
                break;

            case EnemyStates.CHASE:
                ChaseTarget(playerObject.transform);
                isFloating = false;
                break;

            case EnemyStates.ATTACK:
                Attack();
                isFloating = false;
                break;

            case EnemyStates.DEAD:
                break;

            default:
                break;
        }
    }

    private Vector2 FloatVertically() {
        if (isFloating) {
            Debug.Log("floatUp: " + floatUp);
            float distCovered = (Time.time - startTimeY) * moveSpeed;
            float fracJourney = distCovered / floatingDistanceY;
            Vector2 lerpTransform = Vector2.Lerp(currentPositionY, newPositionY, fracJourney);

            if (transform.position.y == newPositionY.y && floatUp) {
                startTimeY = Time.time;
                floatUp = false;
                currentPositionY = new Vector2(0, transform.position.y);
                newPositionY = new Vector2(0, transform.position.y - floatingDistanceY);
            } else if (transform.position.y == newPositionY.y && !floatUp) {
                startTimeY = Time.time;
                floatUp = true;
                currentPositionY = new Vector2(0, transform.position.y);
                newPositionY = new Vector2(0, transform.position.y + floatingDistanceY);
            }

            return lerpTransform;
        } else {
            InitFloatVariables();
            isFloating = true;
            return transform.position;
        }
    }

    private Vector2 FloatHorizontally() {
        if (isFloating) {
            Debug.Log("floatRight: " + floatRight);

            float distCovered = (Time.time - startTimeX) * moveSpeed;
            float fracJourney = distCovered / floatindDistanceX;
            Vector2 lerpPosition = Vector2.Lerp(currentPositionX, newPositionX, fracJourney);

            if (transform.position.x == newPositionX.x && floatRight) {
                startTimeX = Time.time;
                floatRight = false;
                currentPositionX = new Vector2(transform.position.x, 0);
                newPositionX = new Vector2(transform.position.x - floatindDistanceX, 0);
            } else if (transform.position.x == newPositionX.x && !floatRight) {
                startTimeX = Time.time;
                floatRight = true;
                currentPositionX = new Vector2(transform.position.x, 0);
                newPositionX = new Vector2(transform.position.x + floatindDistanceX, 0);
            }

            return lerpPosition;
        } else {
            InitFloatVariables();
            isFloating = true;
            return transform.position;
        }
    }

    private void Alert() {
        float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
        float _temp = _distanceToPlayer;

        // Check if player comes closer >> CHASE, or walks away >> IDLE
        if (_distanceToPlayer > _temp) {
            currentState = EnemyStates.IDLE;
        } else if (_distanceToPlayer < _temp) {
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
