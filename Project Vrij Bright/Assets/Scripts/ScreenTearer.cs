using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ScreenTearer : MonoBehaviour {

    public PostProcessingProfile ppp;
    private ChromaticAberrationModel cAbberate;

    public static ScreenTearer _Instance;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    private void Start(){
        cAbberate = ppp.chromaticAberration;
        ChangeSettingsChromaticAberration(0);
    }

    //sets chromatic aberration to a value _intensity
    //first copies current settings to var to make values writeable
    public void ChangeSettingsChromaticAberration(int _intensity){
        cAbberate = ppp.chromaticAberration;
        var newSettings = cAbberate.settings;
        newSettings.intensity = _intensity;
        ppp.chromaticAberration.settings = newSettings;
    }
}
