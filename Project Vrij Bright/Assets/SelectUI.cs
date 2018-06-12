using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectUI : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

    // Use this for initialization
    void Start() {

        }

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis("HorizontalJ1") != 0 && buttonSelected == false|| Input.GetAxisRaw("HorizontalJ2") != 0 && buttonSelected == false) {
            Debug.Log("Menu");
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
            }
        }

    private void OnDisable() {
        buttonSelected = false;
        }
    }
