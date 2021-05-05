using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Audio : MonoBehaviour
{
    private AudioSource audio_Source;
    private EnemyController enemy_Controller;

    [SerializeField]
    private AudioClip die_Clip, scream_clip;

    [SerializeField]
    private AudioClip[] attack_Clips;

    [SerializeField]
    private AudioClip[] defoult_Clips;

    [SerializeField]
    private AudioClip aware_Clip;


    public float minPitch = 0.05f;
    public float maxPitch = 0.1f;

    void Awake()
    {
        audio_Source = GetComponent<AudioSource>();
        audio_Source.pitch = Random.Range(0.4f, 1.3f);
    }

    public void PlayScreamSoud()
    {
        if (!audio_Source.isPlaying)
        {
            audio_Source.clip = scream_clip;
            audio_Source.Play();
        }
    }

    public void PlayAttackSound()
    {
        audio_Source.clip = attack_Clips[Random.Range(0, attack_Clips.Length)];
        audio_Source.Play();
    }

    public void PlayDeadSound()
    {
        audio_Source.clip = die_Clip;
        audio_Source.Play();
    }

    public void PlayDefaultSound()
    {
        if (!audio_Source.isPlaying)
        {
            audio_Source.clip = defoult_Clips[Random.Range(0, defoult_Clips.Length)];
            audio_Source.Play();
        }
    }

    public void PlayAwareSound()
    {
        if (!audio_Source.isPlaying)
        {
            audio_Source.clip = aware_Clip;
            audio_Source.Play();
        }
    }

    public void StopSound()
    {
        audio_Source.Stop();
    }
}
