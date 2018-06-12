using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour {

    private GameObject openDoors;
    private GameObject closedDoors;
    
	private void Start () {
        openDoors = transform.GetChild(0).gameObject;
        openDoors.SetActive(false);
        closedDoors = transform.GetChild(1).gameObject;
        closedDoors.SetActive(true);
    }
	
    public void HandleDoors(bool _open) {
        if (_open) {
            openDoors.SetActive(true);
            closedDoors.SetActive(false);
        }
        else {
            openDoors.SetActive(false);
            closedDoors.SetActive(true);
        }
    }

}
