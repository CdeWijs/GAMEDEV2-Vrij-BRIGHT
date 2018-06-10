using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class to drop cage when rope is hit
/// </summary>
public class CageScript : MonoBehaviour {

    Rigidbody2D rb2d;
    public GameObject cage;

    public void DropCage() {
        if (cage.gameObject != null) {
            rb2d = cage.gameObject.AddComponent<Rigidbody2D>();
            }
        }
    }
