using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Simple : EnemyBaseClass
{
    private Transform targetTransform;
    public SpriteRenderer spriteRenderer;

    public float amplitudeX = 10.0f;
    public float amplitudeY = 5.0f;
    public float omegaX = 1.0f;
    public float omegaY = 1.0f;

    private float index;

    // FMOD
    [FMODUnity.EventRef]
    public string attackEvent;
    [FMODUnity.EventRef]
    public string deathEvent;
    private bool playedDeathAudio = false;
    [FMODUnity.EventRef]
    public string idleEvent;
    private FMOD.Studio.EventInstance idleInstance;

    new public void Start()
    {
        base.Start();
        //sprR = gameObject.GetComponent<SpriteRenderer>();
        idleInstance = FMODUnity.RuntimeManager.CreateInstance(idleEvent);
        idleInstance.start();
    }
    new public void Update()
    {
        base.Update();
        //EnemyMovement();
        idleInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody2D>()));
    }

    public override void FindPlayer() { } // Don't use base function

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Attack();
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
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
