using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioEmotion : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip[] AudioClips;
    public int PauseBetweenAudioClips = 1;
    public bool ContinuePlaying = true;
    public bool ContinuePlayingSameEmotion = true;
    public bool LoopSameAudioClip = false;
    
    public string[] Emotions;

    public string AudioFolder = "Audio/Emotions";
    private System.Random random = new System.Random();

    private string currentEmotion;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioClips = Resources.LoadAll<AudioClip>(AudioFolder);
        Emotions = AudioClips.Select(a =>
        {
            //get pos of second -
            int index = a.name.IndexOf('-', a.name.IndexOf('-') + 1) + 1;
            string emotion = a.name.Substring(index);
            emotion = emotion.Substring(0, emotion.Length - 2);
            return emotion;
        })
            .Distinct()
            .ToArray();
    }

    public void PlayAudio(string emotion, bool sameAudioClip = false)
    {
        if (!sameAudioClip)
        {
            currentEmotion = emotion.ToLower();
            List<AudioClip> emotionAudioClips = AudioClips.Where(a => a.name.ToLower().Contains(currentEmotion))
                .ToList();

            //don't play same emotion twice
            if (audioSource.clip != null)
                emotionAudioClips.Where(a => a.name != audioSource.clip.name)
                    .ToList();

            audioSource.clip = emotionAudioClips[random.Next(0, emotionAudioClips.Count)];
        }

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ContinuePlayingSameEmotion) LoopSameAudioClip = false;

        if (Input.GetKey(KeyCode.N)) PlayAudio("neutral", LoopSameAudioClip);
        if (Input.GetKey(KeyCode.A)) PlayAudio("anger", LoopSameAudioClip);
        if (Input.GetKey(KeyCode.F)) PlayAudio("fear", LoopSameAudioClip);
        if (Input.GetKey(KeyCode.H)) PlayAudio("happiness", LoopSameAudioClip);
        if (Input.GetKey(KeyCode.S)) PlayAudio("sadness", LoopSameAudioClip);

        CheckCurrentAudio();
    }

    public async void CheckCurrentAudio()
    {
        //current emotion audio stopped?
        if (audioSource.clip != null && !audioSource.isPlaying && ContinuePlaying)
        {
            //pause
            await Task.Delay(TimeSpan.FromSeconds(PauseBetweenAudioClips));

            if (ContinuePlayingSameEmotion)
            {
                PlayAudio(currentEmotion, LoopSameAudioClip);
                return;
            }

            //new emotion
            string newEmotion = Emotions
                    .Where(e => e != currentEmotion) //filter out current emotion
                    .ElementAt(random.Next(0, Emotions.Length - 1)); //choose randomly new emotion

            PlayAudio(newEmotion);
        }
    }
}
