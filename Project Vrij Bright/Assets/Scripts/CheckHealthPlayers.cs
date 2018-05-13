using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// manager script, work in progress.
/// </summary>
public class CheckHealthPlayers : MonoBehaviour {

    public float boyHealth;
    public bool boyIsScared = false;

    public float guardianHealth;
    public float flyPowerLeft;

    //reference for singleton
    public static CheckHealthPlayers _Instance;

    private void Awake()
    {
        #region Singleton
        if (_Instance == null)
        {
            _Instance = this;
        }
        else if (_Instance != this)
        {
            Destroy(this);
        }
        #endregion
    }




}
