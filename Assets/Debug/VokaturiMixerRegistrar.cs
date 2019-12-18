using System.Collections.Generic;
using SMILEI.Vokaturi;
using UnityEngine;

public class VokaturiMixerRegistrar : MonoBehaviour
{
    public List<VokaturiMixerAsset> Mixers = new List<VokaturiMixerAsset>();
    
    void Awake()
    {
        foreach (var mixer in Mixers)
        {
            ((VokaturiMixer)mixer.Implementation).Register();
        }
    }

    void OnDestroy()
    {
        foreach (var mixer in Mixers)
        {
            ((VokaturiMixer)mixer.Implementation).Unregister();
        }
    }
}
