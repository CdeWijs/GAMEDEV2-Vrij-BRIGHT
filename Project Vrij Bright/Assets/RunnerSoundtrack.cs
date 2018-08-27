using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerSoundtrack : MonoBehaviour {

    public BoyClass boyClass;
    public GameObject endSoundtrack;

    private void Update()
    {
        if (boyClass.health <= 15)
        {
            endSoundtrack.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
