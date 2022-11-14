using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEngine : MonoBehaviour
{

    public static AudioEngine instance = null;
    public AudioSource musicStream;
    public AudioSource soundStream;
    public UnityEvent weakTempoEvent;
    public UnityEvent strongTempoEvent;

    [SerializeField] int bpm;
    float secondPerBeat;
    float chrono;
    float nextBeatTime;
    [SerializeField] float offset;

    public AudioLowPassFilter lowPass;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip soundClipToPlay, bool loop)
    {
        musicStream.clip = soundClipToPlay;
        musicStream.Play();
        musicStream.loop = loop;
    }

    public void PlaySound(AudioClip soundClipToPlay, bool loop)
    {
        soundStream.clip = soundClipToPlay;
        soundStream.Play();
        soundStream.loop = loop;
    }

    private void Start()
    {
        secondPerBeat = 60f / bpm;
        lowPass = GetComponent<AudioLowPassFilter>();
    }


    private void Update()
    {
        chrono = musicStream.time - offset;
        if (chrono >= nextBeatTime)
        {
            weakTempoEvent.Invoke();
            nextBeatTime += secondPerBeat;
        }
    }



}
