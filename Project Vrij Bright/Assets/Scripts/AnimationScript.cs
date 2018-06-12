﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationScript : MonoBehaviour {

    public AnimationClip aC;
    public Animator an;
    public int scene;
    private float time;
    private bool playing = false;
    private Fade fader;
    public bool fadeInOrOut;

    private void Start() {
        fader = GetComponent<Fade>();
        time = aC.length / an.speed;
        }

    private void Update() {
        if (!Conversation._Instance.playing) {
            an.SetBool("Ready", true);
            StartCoroutine(SetNextScene(time, scene));

            }
        }

    private IEnumerator SetNextScene(float _time, int _scene) {
        Fader();
        yield return new WaitForSeconds(_time);
        SceneManager.LoadScene(_scene);
        }
    // the image you want to fade, assign in inspector
    public Image img;

    public void Fader() {
        // fades the image out when you click
        StartCoroutine(FadeImage(fadeInOrOut));
        }

    IEnumerator FadeImage(bool fadeAway) {
        // fade from opaque to transparent
        if (fadeAway) {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime/ 5) {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i *2 );
                yield return null;
                }
            }
        // fade from transparent to opaque
        else {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime/5) {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i *2);
                yield return null;
                }
            }
        }


    }