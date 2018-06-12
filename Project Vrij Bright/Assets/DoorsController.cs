using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour {

    private GameObject openDoors;
    private GameObject closedDoors;

    // FMOD
    [FMODUnity.EventRef]
    public string doorOpen;
    private FMOD.Studio.EventInstance instanceOpen;
    [FMODUnity.EventRef]
    public string doorClose;
    private FMOD.Studio.EventInstance instanceClose;


    private void Start () {
        openDoors = transform.GetChild(0).gameObject;
        openDoors.SetActive(false);
        closedDoors = transform.GetChild(1).gameObject;
        closedDoors.SetActive(true);

        instanceOpen = FMODUnity.RuntimeManager.CreateInstance(doorOpen);
        instanceClose = FMODUnity.RuntimeManager.CreateInstance(doorClose);
    }

    public void HandleDoors(bool _open) {
        if (_open) {
            openDoors.SetActive(true);
            closedDoors.SetActive(false);
            instanceOpen.start();
        }
        else {
            openDoors.SetActive(false);
            closedDoors.SetActive(true);
            instanceClose.start();
        }
    }

}
