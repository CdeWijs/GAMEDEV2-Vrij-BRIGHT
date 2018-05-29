using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAnimation : MonoBehaviour {

    public GameObject vortex;

    private void Update(){
        float multiplier = Mathf.Abs(Mathf.Sin(vortex.transform.localScale.x));
        vortex.transform.localScale = new Vector3(vortex.transform.localScale.x * multiplier, vortex.transform.localScale.y * multiplier);
        vortex.transform.localEulerAngles = new Vector3(vortex.transform.localEulerAngles.x, vortex.transform.localEulerAngles.y, vortex.transform.localEulerAngles.z +1);
    }
}
