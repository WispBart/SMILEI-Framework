using SMILEI.Vokaturi;
using UnityEngine;
using UnityEngine.UI;

public class VokaturiToText : MonoBehaviour
{

    public VokaturiMixerAsset Mixer;
    public Text Display;

    void Update()
    {
        Display.text = Mixer.Implementation.GetValue().Value.ToString();
    }
}
