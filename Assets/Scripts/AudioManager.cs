using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] AudioSource soundPrefab = default;
    [SerializeField] AudioSource musicAudio = default;
    Pooling<AudioSource> poolingSound = new Pooling<AudioSource>();

    [Header("Music")]
    [SerializeField] AudioClip menuMusic = default;

    [Header("Sounds")]
    [SerializeField] AudioClip clickButton = default;

    void Awake()
    {
        //singleton
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic()
    {
        //menu music
        musicAudio.clip = menuMusic;
        musicAudio.Play();
    }

    public void PlaySound()
    {
        //instantiate sound
        AudioSource soundAudio = instance.poolingSound.Instantiate(soundPrefab, instance.transform);
        soundAudio.transform.position = instance.transform.position;

        //click button
        soundAudio.clip = instance.clickButton;
        soundAudio.Play();

        //stop after few seconds
        instance.StartCoroutine(instance.StopSound(soundAudio, clickButton.length));
    }

    public static void PlayMusic(AudioClip clip)
    {
        //music 
        instance.musicAudio.clip = clip;
        instance.musicAudio.Play();
    }

    public static void PlaySound(AudioClip clip)
    {
        //instantiate sound
        AudioSource soundAudio = instance.poolingSound.Instantiate(instance.soundPrefab, instance.transform);
        soundAudio.transform.position = instance.transform.position;

        //set clip and play
        soundAudio.clip = clip;
        soundAudio.Play();

        //stop after few seconds
        instance.StartCoroutine(instance.StopSound(soundAudio, clip.length));
    }

    public IEnumerator StopSound(AudioSource soundAudioSource, float timer)
    {
        //wait
        yield return new WaitForSeconds(timer);

        //deactive
        Pooling.Destroy(soundAudioSource.gameObject);
    }
}
