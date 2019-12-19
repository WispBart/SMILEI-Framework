using System;
using System.Collections;
using System.Collections.Generic;
using SMILEI.Vokaturi;
using UnityEngine;

public class VokaturiOutputLogger : MonoBehaviour
{
    [SerializeField] private List<VokaturiMixerAsset> Mixer;

    [SerializeField] private int _roundingDigits = 3;

    void OnEnable()
    {
        foreach (var m in Mixer)
        {
            var mixer = ((VokaturiMixer)m.Implementation);
            mixer.Register();
            mixer.DataEvent.AddListener((value) => OnReceive(m, value));
        }
    }
    private void OnDisable()
    {
        foreach (var m in Mixer)
        {
            var mixer = ((VokaturiMixer)m.Implementation);
            mixer.Unregister();
            mixer.DataEvent.RemoveListener((value) => OnReceive(m, value));
        }
    }

    private void OnReceive(VokaturiMixerAsset m, double value)
    {
        if (!isActiveAndEnabled) return;
        
        Debug.Log($"{m.name} reports:\nValue at {Math.Round(Time.time, _roundingDigits)} seconds since play: {Math.Round(value,_roundingDigits)}");
    }


    
}
