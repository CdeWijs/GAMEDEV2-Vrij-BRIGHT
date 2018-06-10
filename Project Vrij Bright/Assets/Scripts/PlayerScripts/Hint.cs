using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class to pass hint text to conversation class
/// </summary>
public class Hint : MonoBehaviour {

    // public GameObject hintObject;
    public string[] hintText;

    public void SetHintActive() {
        Conversation._Instance.DisplayHint(hintText);
        }
    }
