using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;


    public AudioSource audioSourcePrefab;
    public int initialPoolSize = 10;
    private List<AudioSource> pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pool = new List<AudioSource>(initialPoolSize);
        for (int i = 0; i < initialPoolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.gameObject.SetActive(false);
            pool.Add(source);
        }
    }

    public void PlaySFX(SFXData data)
    {
        PlaySound(data.sfxClip, data.volume, data.pitch);
    }

    private void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        foreach (var source in pool)
        {
            if (source.isPlaying == false)
            {
                PlayWithSource(source, clip, volume, pitch);
                return;
            }
        }

        AudioSource newSource = Instantiate(audioSourcePrefab, transform);
        pool.Add(newSource);
        PlayWithSource(newSource, clip, volume, pitch);
    }

    private void PlayWithSource(AudioSource source, AudioClip clip, float volume, float pitch)
    {
        source.pitch = pitch;
        source.gameObject.SetActive(true);
        source.PlayOneShot(clip, volume);
        StartCoroutine(DisableAfterPlay(source, clip.length));
    }

    private IEnumerator DisableAfterPlay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.SetActive(false);
    }
}