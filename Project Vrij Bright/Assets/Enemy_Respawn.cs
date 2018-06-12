using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Respawn : MonoBehaviour {

    public GameObject playerObject;
    public float respawnRadius = 5;

    private bool dead;
    private bool puzzleSolved = false;

    public void Die() {
        if (puzzleSolved) {
            Destroy(gameObject);
        } else {
            gameObject.SetActive(false);
        }
        dead = true;
    }

    private void Update() {
        if (dead) {
            float _distanceToPlayer = Mathf.Abs((playerObject.transform.position.x - transform.position.x));
            if (_distanceToPlayer > respawnRadius) {
                gameObject.SetActive(true);
            }
        }
    }
}
