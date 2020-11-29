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
        AudioManager.PlayMusic(instance.menuMusic);
    }

    public void PlaySound()
    {
        AudioManager.PlaySound(instance.clickButton);
    }

    public static void PlayMusic(AudioClip clip)
    {
        //do only if music active
        if (FunctionsUI.instance.Music)
        {
            //music 
            instance.musicAudio.clip = clip;
            instance.musicAudio.Play();
        }
    }

    public static void PlaySound(AudioClip clip)
    {
        //do only if sound active
        if (FunctionsUI.instance.Sound)
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
    }

    public IEnumerator StopSound(AudioSource soundAudioSource, float timer)
    {
        //wait
        yield return new WaitForSeconds(timer);

        //deactive
        Pooling.Destroy(soundAudioSource.gameObject);
    }
}
