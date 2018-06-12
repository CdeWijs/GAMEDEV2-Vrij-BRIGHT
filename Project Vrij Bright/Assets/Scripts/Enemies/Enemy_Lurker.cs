using UnityEngine;

/// <summary>
/// class for the lurker type enemy: enemy is invincible while in shadows, but can be attacked when it comes out of hiding by using bait
/// </summary>
public class Enemy_Lurker : EnemyBaseClass {

    public GameObject bait;
    public bool baitOnGround = false;
    public bool isInShadows = true;
    public GameObject lt;

    private Transform targetTransform;
    private float baitRadius = 30f;

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.ParameterInstance monsterStatus;
    private float dwalen = 0;
    private float verschrikt = 20;
    private float aanvallen = 30;
    private float dood = 50;

    private bool teleported = false;

    new private void Start() {
        base.Start();
        // bait = GameObject.FindGameObjectWithTag("Bait");
        currentState = EnemyStates.IDLE;

        // FMOD
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, this.gameObject.transform, rigidBody2D);
        instance.start(); ;
        instance.getParameter("MonsterStatus", out monsterStatus);
        monsterStatus.setValue(dwalen);
    }

    //does not call base update because of statemachine
    new private void Update() {
        //base.Update();
        FindPlayer();
        FindBait();
        StateMachine(currentState);
        SetLight();

        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
    }

    public void SetLight() {
        if (isInShadows) {
            // lt.SetActive(true);
        } else {
            lt.SetActive(false);
        }
    }
    //enemy is invulnerable in shadows
    public override void CheckHealth() {
        if (!isInShadows) {
            if (enemyHealth <= 0) {
                monsterStatus.setValue(dood);
                Destroy(this.gameObject);
            }
        }
    }


    public override void TakeDamage(int amount) {
        if (!isInShadows) {
            base.TakeDamage(amount);
        }
    }
    //enemy moves towards target 
    public override void EnemyMovement() {
        Vector3 moveToPos = new Vector3(targetTransform.transform.position.x, transform.position.y, 0);
        transform.position = Vector2.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
    }

    //checks if bait is on the ground and if enemy should come out of hiding
    private void FindBait() {
        if (bait.gameObject == null) {
            return;
        }

        float _distanceToBait = Mathf.Abs((bait.transform.position.x - transform.position.x));
        if (Mathf.RoundToInt(bait.transform.position.y) == Mathf.RoundToInt(transform.position.y)) {
            currentState = EnemyStates.CHASEBAIT;
        }
        else if (_distanceToBait < baitRadius) {
            currentState = EnemyStates.CHASEBAIT;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Bait") {
            EatBait(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Shadow") {
            isInShadows = true;
        }
        if (collision.gameObject.tag == "Mirror") {
            if (!teleported) {
                collision.gameObject.GetComponent<Interaction>().Teleport(this.gameObject);
                teleported = true;
            }
        }
    }

    //check if enemy is in shadows 
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Shadow") {
            isInShadows = false;
        }
    }

    //enemy eats bait and returns to idle state
    private void EatBait(GameObject bait) {
        baitOnGround = false;
        isInShadows = false;
        currentState = EnemyStates.IDLE;
        transform.position = bait.transform.position;
        Destroy(bait);
    }

    //state machine for lurker enemy
    private void StateMachine(EnemyStates state) {
        switch (state) {
            case EnemyStates.NULL:
                break;

            case EnemyStates.IDLE:
                //perhaps Idle animation
                break;

            case EnemyStates.CHASEBAIT:
                targetTransform = bait.transform;
                EnemyMovement();
                break;

            case EnemyStates.CHASE:
                targetTransform = playerObject.transform;
                EnemyMovement();
                break;

            case EnemyStates.ATTACK:
                targetTransform = null;
                Attack();
                break;

            default:
                break;
        }
    }
}
