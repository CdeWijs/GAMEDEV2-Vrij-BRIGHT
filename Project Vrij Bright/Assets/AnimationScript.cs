using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class animationScript : MonoBehaviour {

    public AnimationClip aC;
    public Animator an;

    public string scene = "Introduction";
    public Color loadToColor = Color.black;
    private void Start() {
        float time = aC.length / an.speed;
        StartCoroutine(SetNextScene(time, scene));
        }

    private IEnumerator SetNextScene(float _time, string _scene) {
        yield return new WaitForSeconds(_time);
        Initiate.Fade(scene, loadToColor, 0.5f);
        }
    }
