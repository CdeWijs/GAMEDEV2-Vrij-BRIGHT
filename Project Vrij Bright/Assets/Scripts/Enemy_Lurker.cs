using UnityEngine;

/// <summary>
/// class for the lurker type enemy: enemy is invincible while in shadows, but can be attacked when it comes out of hiding by using bait
/// </summary>
public class Enemy_Lurker : EnemyBaseClass {

    public GameObject bait;
    public bool baitOnGround = false;
    public bool isInShadows = true;
    public float attackRadius, chaseRadius;
    public GameObject lt;

    private Transform targetTransform;
    private enum LurkerStates { idle, chasePlayer, attack, chaseBait };
    private LurkerStates enemyState;

    new private void Start() {
        base.Start();
       // bait = GameObject.FindGameObjectWithTag("Bait");
        enemyState = LurkerStates.idle;
    }

    //does not call base update because of statemachine
    new private void Update() {
        //base.Update();
        FindPlayer();
        FindBait();
        StateMachine(enemyState);
        SetLight();
    }

    public void SetLight()
    {
        if (isInShadows)
        {
            lt.SetActive(true);
        }
        else
        {
            lt.SetActive(false);
        }
    }
    //enemy is invulnerable in shadows
    public override void CheckHealth(){
        if (!isInShadows){
            if (enemyHealth <= 0){
                Destroy(this.gameObject);
            }
        }
    }


    public override void TakeDamage(int amount){
        if (!isInShadows){
            base.TakeDamage(amount);
        }
    }
    //enemy moves towards target 
    public override void EnemyMovement() {
        Vector3 moveToPos = new Vector3(targetTransform.transform.position.x, transform.position.y, 0);
        transform.position = Vector2.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
    }

    //checks if player is in range for chasing or attacking
    public override void FindPlayer() {
        base.FindPlayer();

        //sets enemy back to idle when player is killed, can be used if we decide to add some sort of player revival 
        if (playerObject == null) {
            enemyState = LurkerStates.idle;
            return;
        }

        float distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));

        if (distanceToPlayer < attackRadius) {
            enemyState = LurkerStates.attack;
        } else if (distanceToPlayer < chaseRadius && !isInShadows) {
            enemyState = LurkerStates.chasePlayer;
        } else {
            enemyState = LurkerStates.idle;
        }
    }

    //checks if bait is on the ground and if enemy should come out of hiding
    private void FindBait() {
        if (bait.gameObject == null) {
            return;
        }

        if (Mathf.RoundToInt(bait.transform.position.y) == Mathf.RoundToInt(transform.position.y)) {
            enemyState = LurkerStates.chaseBait;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Bait") {
            EatBait(collision.gameObject);
        }
    }

    //check if enemy is in shadows 
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.tag == "Shadow"){
            isInShadows = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Shadow"){
            isInShadows = true;
        }
    }

    //enemy eats bait and returns to idle state
    private void EatBait(GameObject bait) {
        baitOnGround = false;
        isInShadows = false;
        enemyState = LurkerStates.idle;
        transform.position = bait.transform.position;
        Destroy(bait);
    }

    //direct reference to boy's health, needs to be replace for actual attacking
    private void Attack() {
        playerObject.GetComponent<BoyClass>().health -= 1;
    }

    //state machine for lurker enemy
    private void StateMachine(LurkerStates state) {
        switch (state) {
            case LurkerStates.idle:
                //perhaps Idle animation
                break;

            case LurkerStates.chaseBait:
                targetTransform = bait.transform;
                EnemyMovement();
                break;

            case LurkerStates.chasePlayer:
                targetTransform = playerObject.transform;
                EnemyMovement();
                break;

            case LurkerStates.attack:
                targetTransform = null;
                Attack();
                break;

            default:
                break;
        }
    }
}
