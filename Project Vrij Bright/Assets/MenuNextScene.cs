using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuNextScene : MonoBehaviour {

    public Image img;
    public bool fadeBool;

    public void LoadByIndex(int sceneIndex) {
        img.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeImage(fadeBool, sceneIndex));
        }
    IEnumerator FadeImage(bool fadeAway, int sceneIndex) {
        // fade from opaque to transparent
        if (fadeAway) {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);
                }
            SceneManager.LoadScene(sceneIndex);
            yield return null;
            }

        // fade from transparent to opaque
        else {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime) {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);
                }
            SceneManager.LoadScene(sceneIndex);
            yield return null;
            }
        }
    }

