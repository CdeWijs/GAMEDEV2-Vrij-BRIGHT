using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLight : MonoBehaviour {

    public Light lt;
    public float multiplier;

    private void Update(){
        lt.intensity = multiplier *  Mathf.Abs(Mathf.Cos(Time.time * 2.5f));
    }
}
