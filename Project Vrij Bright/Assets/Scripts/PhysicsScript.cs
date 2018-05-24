using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class contains static functions for dream physics
/// </summary>
public class PhysicsScript : MonoBehaviour{

    //set gravity and scale to increased values of the BOY
    public static void GravityIncrease(GameObject _player, float _scaleMultiplier, float _gravityMultiplier){
        Vector3 newSize = _player.transform.localScale;
        newSize = new Vector3(newSize.x, newSize.y * _scaleMultiplier, newSize.z);
        _player.transform.localScale = newSize;
        _player.GetComponent<Rigidbody2D>().gravityScale *= _gravityMultiplier;
        ScreenTearer._Instance.ChangeSettingsChromaticAberration(1);
    }

    //resets gravity and scale of the BOY
    public static void ResetGravity(GameObject _player){
        _player.transform.localScale = new Vector3(1, 1, 1);
        _player.GetComponent<Rigidbody2D>().gravityScale = 1;
        ScreenTearer._Instance.ChangeSettingsChromaticAberration(0);
    }

    //w.i.p. 
    public static void LaunchPlayer(GameObject _player, float _force, Vector3 _dir){
        _player.GetComponent<Rigidbody2D>().AddForce(_dir * _force);
    }

    //use this to change jumpForce, speed, gravity etc
    public static float EffectedFloat(float _value, float _multiplier = 1)
    {
        return _value * _multiplier;
    }
}