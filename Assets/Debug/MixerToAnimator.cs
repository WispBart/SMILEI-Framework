using System.Collections;
using System.Collections.Generic;
using SMILEI.Core;
using UnityEngine;

public class MixerToAnimator : MonoBehaviour
{
    public EmotionMixerAsset Mixer;
    public Animator Animator;
    public string AnimatorKey;

    void Update()
    {
        Animator.SetFloat(AnimatorKey, Mixer.GetValue().Value);
    }
}
