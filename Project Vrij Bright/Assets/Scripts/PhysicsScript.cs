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
    }

    //resets gravity and scale of the BOY
    public static void ResetGravity(GameObject _player){
        _player.transform.localScale = new Vector3(1, 1, 1);
        _player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    //change speed of BOY
    public static void ChangeSpeedBoy(GameObject _player, float _speed){
        BoyController _bC = _player.GetComponent<BoyController>();
        _bC.currentSpeed = _speed;
    }

    //w.i.p. 
    public static void LaunchPlayer(GameObject _player, float _force, Vector3 _dir){
        _player.GetComponent<Rigidbody2D>().AddForce(_dir * _force);
    }
}