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
    }

    public void PlaySound(AudioClip soundClipToPlay, bool loop)
    {
        soundStream.clip = soundClipToPlay;
        soundStream.Play();
    }

    private void Start()
    {
        secondPerBeat = 60f / bpm;
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
