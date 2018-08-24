using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// baseclass for enemies to derrive from
/// </summary>
public class EnemyBaseClass : MonoBehaviour
{

    public enum EnemyStates
    {
        NULL,
        IDLE,
        CHASEBAIT,
        ALERT,
        CHASE,
        ATTACK,
        CONTENT,
        DEAD
    }

    public EnemyStates currentState;
    public int enemyHealth;
    public float moveSpeed, chaseSpeed;
    public float alertRadius, chaseRadius, attackRadius;
    public float attackRate = 1f;
    public float nextAttack;

    public Rigidbody2D rigidBody2D;
    public GameObject playerObject;
    public Slider enemyHealthSlider;

    protected bool findPlayer = true;

    public void Start()
    {
        currentState = EnemyStates.IDLE;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyHealthSlider.maxValue = enemyHealth;
        enemyHealthSlider.value = enemyHealth;
    }

    public void Update()
    {
        if (findPlayer)
        {
            FindPlayer();
        }
        CheckHealth();
        enemyHealthSlider.value = enemyHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        enemyHealth -= amount;
    }

    public virtual void EnemyMovement()
    {
        //add behaviour on derriving enemy
    }

    public virtual void FindPlayer()
    {
        if (playerObject == null)
        {
            currentState = EnemyStates.IDLE;
        }
        else
        {
            float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
            if (_distanceToPlayer < attackRadius)
            {
                currentState = EnemyStates.ATTACK;
            }
            else if (_distanceToPlayer < chaseRadius)
            {
                currentState = EnemyStates.CHASE;
            }
            else if (_distanceToPlayer < alertRadius)
            {
                currentState = EnemyStates.ALERT;
            }
            else
            {
                currentState = EnemyStates.IDLE;
            }
        }
    }

    public virtual void CheckHealth()
    {
        Debug.Log(enemyHealth);
        if (enemyHealth <= 0)
        {
            currentState = EnemyStates.DEAD;
            Destroy(this.gameObject);
        }
    }

    public virtual void ChaseTarget(Transform _target)
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, chaseSpeed * Time.deltaTime);
    }

    public virtual void Attack()
    {
        if (Time.time > nextAttack)
        {
            playerObject.GetComponent<BoyClass>().health -= 8;
            nextAttack = Time.time + attackRate;
        }
    }
}
