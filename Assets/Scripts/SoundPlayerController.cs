using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerController : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;

    [SerializeField] protected float interval;

    void Start()
    {
        InvokeRepeating(nameof(PlaySound), Random.Range(0, 5), interval);
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}
