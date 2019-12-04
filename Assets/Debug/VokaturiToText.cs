using System.Collections;
using System.Collections.Generic;
using SMILEI.Vokaturi;
using UnityEngine;
using UnityEngine.UI;

public class VokaturiToText : MonoBehaviour
{

    public VokaturiMixerAsset Mixer;
    public Text Display;
    
    void OnEnable()
    {
        (Mixer.Implementation as VokaturiMixer)?.Register();
    }

    void OnDisable()
    {
        (Mixer.Implementation as VokaturiMixer)?.Unregister();

    }

    void Update()
    {
        Display.text = Mixer.Implementation.GetValue().Value.ToString();
    }
}
