using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ScreenTearer : MonoBehaviour {

    public PostProcessingProfile ppp;
    private ChromaticAberrationModel cAbberate;
    private ColorGradingModel cg;

    public bool Up;
    private int currentHue = 0;
    public static ScreenTearer _Instance;
    private float randomTime;
    private float timer = 0;
    private void Awake() {
        if (_Instance == null) {
            _Instance = this;
            } else {
            Destroy(this);
            }
        }

    private void Start() {
        cg = ppp.colorGrading;
        cAbberate = ppp.chromaticAberration;
        ChangeSettingsChromaticAberration(0);
        randomTime = Random.Range(0, 20);
        }


    public void Update() {

        if (timer < randomTime) {
            timer += Time.deltaTime;
            } else {
            if (currentHue >= 180) {
                Up = false;
                }
            if (currentHue <= -180) {
                Up = true;
                }
            if (Up) {
                currentHue++;

                } else { currentHue--; }
            ChangeHue(currentHue);
            }
        }





    public void ChangeHue(int _shift) {
        var newSettings = cg.settings;
        newSettings.basic.hueShift = _shift;
        cg.settings = newSettings;
        }

    //sets chromatic aberration to a value _intensity
    //first copies current settings to var to make values writeable
    public void ChangeSettingsChromaticAberration(int _intensity) {
        cAbberate = ppp.chromaticAberration;
        var newSettings = cAbberate.settings;
        newSettings.intensity = _intensity;
        ppp.chromaticAberration.settings = newSettings;
        }
    }
