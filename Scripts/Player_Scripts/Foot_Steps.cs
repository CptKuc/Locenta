using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_Steps : MonoBehaviour
{
    private AudioSource footsteps_Sound;

    [SerializeField]
    private AudioClip[] footsteps_Clip;

    private CharacterController character_Controllor;

    [HideInInspector]
    public float volume_Min, volume_Max;

    private float accumulated_Distance;
    public float step_Distance;
    private float jump_Volume = 1f;
    public bool in_Air = false;
    // Start is called before the first frame update
    public GameObject player;
    void Awake()
    {
        footsteps_Sound = GetComponent<AudioSource>();
        character_Controllor = GetComponentInParent<CharacterController>();
        player = GameObject.FindWithTag(Tags.PLAYER_TAG);
    }

    // Update is called once per frame
    void Update()
    {
        Check_Spund_Play();
    }

    void Check_Spund_Play()
    {
        if (in_Air && character_Controllor.isGrounded)
        {
            in_Air = false;
            footsteps_Sound.volume = jump_Volume;
            footsteps_Sound.clip = footsteps_Clip[Random.Range(0, footsteps_Clip.Length)];
            footsteps_Sound.Play();
            if (footsteps_Sound.volume >= 0.1f)
            {
                player.GetComponent<Enemy_Notifier>().Notify(transform.position, 20f);
            }
        }

        if ((Input.GetKeyDown(KeyCode.W) || 
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.A)) && character_Controllor.isGrounded)
        {
            footsteps_Sound.volume = Random.Range(volume_Min, volume_Max);
            footsteps_Sound.clip = footsteps_Clip[Random.Range(0, footsteps_Clip.Length)];
            footsteps_Sound.Play();
            if (footsteps_Sound.volume >= 0.1f)
            {
                player.GetComponent<Enemy_Notifier>().Notify(transform.position, 20f);
            }
        }

        if (!character_Controllor.isGrounded)
        {
            return;
        }
        if (character_Controllor.velocity.sqrMagnitude > 0)
        {
            accumulated_Distance += Time.deltaTime;
            if(accumulated_Distance > step_Distance)
            {
                footsteps_Sound.volume = Random.Range(volume_Min, volume_Max);
                footsteps_Sound.clip = footsteps_Clip[Random.Range(0, footsteps_Clip.Length)];
                footsteps_Sound.Play();
                if (footsteps_Sound.volume >= 0.1f)
                {
                    player.GetComponent<Enemy_Notifier>().Notify(transform.position, 20f);
                }

                accumulated_Distance = 0;
            }
        }
        else
        {
            accumulated_Distance = 0;
        }
    }
}
