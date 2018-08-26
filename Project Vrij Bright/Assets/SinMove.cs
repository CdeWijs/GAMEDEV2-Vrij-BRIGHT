using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMove : MonoBehaviour {
    public float moveSpeed;
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        newPosition.x += Mathf.Sin(Time.time) * Time.deltaTime *  Random.Range(-1, 1) ;
        transform.position = newPosition;
        }
}
