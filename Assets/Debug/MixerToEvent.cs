using System;
using System.Collections;
using System.Collections.Generic;
using SMILEI.Core;
using UnityEngine;
using UnityEngine.Events;

public class MixerToEvent : MonoBehaviour
{
    [Serializable] public class FloatEvent : UnityEvent<float> { }

    public EmotionMixerAsset Mixer;
    public FloatEvent OnUpdate = new FloatEvent();


    // Update is called once per frame
    void Update()
    {
        if (Mixer == null) return;
        OnUpdate.Invoke(Mixer.GetValue().Value);
    }
}
