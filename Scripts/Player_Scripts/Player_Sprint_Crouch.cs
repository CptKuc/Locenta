using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sprint_Crouch : MonoBehaviour
{
    private Player_Movement playerMovement;

    public float sprint_Speed = 10f;
    public float move_Speed = 5f;
    public float crouch_Speed = 2f;

    private Transform look_Root;
    private float stand_Height = 0.6f;
    private float crouch_Height = 0f;

    private bool is_Crouching;

    private Foot_Steps player_Footsteps;
    private float sprint_Volume = 0.2f;
    private float crouch_Volume = 0.03f;
    private float walk_Volume_Min = 0.1f;
    private float walk_Volume_Max = 0.15f;
    private float walk_step_Distance = 0.8f;
    private float sprint_step_Distance = 0.3f;
    private float crouch_step_Distance = 1.4f;

    private Player_Stats player_Stats;
    private float sprint_Value = 100f;
    public float sprint_Trushold = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = GetComponent<Player_Movement>();
        look_Root = transform.GetChild(0);

        player_Footsteps = GetComponentInChildren<Foot_Steps>();

        player_Stats = GetComponent<Player_Stats>();
    }

    void Start()
    {
        player_Footsteps.volume_Min = walk_Volume_Min;
        player_Footsteps.volume_Max = walk_Volume_Max;
        player_Footsteps.step_Distance = walk_step_Distance;
        //set volume and interval for steps sound
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if (sprint_Value > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
            {
                playerMovement.speed = sprint_Speed;
                //set movement speed
                player_Footsteps.step_Distance = sprint_step_Distance;
                player_Footsteps.volume_Min = sprint_Volume;
                player_Footsteps.volume_Max = sprint_Volume;
                //set volume and interval for steps sound
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = move_Speed;
            //set movement speed

            player_Footsteps.step_Distance = walk_step_Distance;
            player_Footsteps.volume_Min = walk_Volume_Min;
            player_Footsteps.volume_Max = walk_Volume_Max;
            //set volume and interval for steps sound
        }

        if(Input.GetKey(KeyCode.LeftShift) && !is_Crouching)
        {
            sprint_Value -= sprint_Trushold * Time.deltaTime;

            if(sprint_Value <= 0f)
            {
                sprint_Value = 0f;
                playerMovement.speed = move_Speed;
                player_Footsteps.step_Distance = walk_step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;
            }
            player_Stats.DisplayStaminaStats(sprint_Value);
        }
        else
        {
            if(sprint_Value != 100f)
            {
                sprint_Value += (sprint_Trushold / 2f) * Time.deltaTime;
                player_Stats.DisplayStaminaStats(sprint_Value);

                if (sprint_Value > 100f)
                {
                    sprint_Value = 100f;
                }
            }
        }

    }//sprint function

    void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (is_Crouching)
            {
                look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
                playerMovement.speed = move_Speed;
                //set movement speed

                
                player_Footsteps.step_Distance = walk_step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;
                //set volume and interval for steps sound

                is_Crouching = false;
            }
            else
            {
                
                look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);
                playerMovement.speed = crouch_Speed;
                //set movement speed

                player_Footsteps.step_Distance = crouch_step_Distance;
                player_Footsteps.volume_Min = crouch_Volume;
                player_Footsteps.volume_Max = crouch_Volume;
                //set volume and interval for steps sound

                is_Crouching = true;
            }
        }
    }//crouch function
}
