using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class contains static functions for dream physics
/// </summary>
public class PhysicsScript : MonoBehaviour {
    static public PhysicsScript _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            } else {
            Destroy(this);
            }
        }

    public static IEnumerator LerpUp(Vector3 _startScale, Vector3 _targetScale, float _time) {
        float progress = 0;

        while (progress <= 1) {
            _startScale = Vector3.Lerp(_startScale, _targetScale, progress);
            progress += Time.deltaTime * _time;
            yield return null;
            }
        _startScale = _targetScale;

        }

    //set gravity and scale to increased values of the BOY
    public static void GravityIncrease(GameObject _player, float _scaleMultiplier, float _gravityMultiplier) {
        Vector3 newSize = _player.transform.localScale;
        newSize = new Vector3(newSize.x, newSize.y * _scaleMultiplier, newSize.z);
         _player.transform.localScale = newSize;
        //_instance.StartCoroutine(LerpUp(_player.transform.localScale, newSize, 0.5f));
        _player.GetComponent<Rigidbody2D>().gravityScale *= _gravityMultiplier;
        ScreenTearer._Instance.ChangeSettingsChromaticAberration(1);
        }

    //resets gravity and scale of the BOY
    public static void ResetGravity(GameObject _player, Vector3 _scale) {
        _player.transform.localScale = _scale;
        _player.GetComponent<Rigidbody2D>().gravityScale = 1;
        //ScreenTearer._Instance.ChangeSettingsChromaticAberration(0);
        }

   

    //w.i.p. 
    public static void LaunchPlayer(GameObject _player, float _force, Vector3 _dir) {
        _player.GetComponent<Rigidbody2D>().AddForce(_dir * _force);
        }

    //use this to change jumpForce, speed, gravity etc
    public static float EffectedFloat(float _value, float _multiplier = 1) {
        return _value / _multiplier;
        }
    }