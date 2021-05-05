using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Foot_Steps player_Footsteps;
    private CharacterController character_Controller;
    private Vector3 move_Direction;

    public float speed = 5f;
    public float gravity = 20f;
    public float jumb_Force = 10f;

    private float vertical_Velocity;

    private void Awake()
    {
        character_Controller = GetComponent<CharacterController>();

        player_Footsteps = GetComponentInChildren<Foot_Steps>();
    }

    // Update is called once per frame
    void Update()
    {
        moveThePlayer();
    }

    void moveThePlayer()
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
                                         Input.GetAxis(Axis.VERTICAL));

        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;

        ApplyGravity();

        character_Controller.Move(move_Direction);
    } //move player

    void ApplyGravity()
    {
        if (character_Controller.isGrounded)
        {
            vertical_Velocity -= gravity * Time.deltaTime;
            PlayerJump();
        }
        else
        {
            vertical_Velocity -= gravity * Time.deltaTime;
        }

        move_Direction.y = vertical_Velocity * Time.deltaTime;
    }// apply gravity

    void PlayerJump()
    {
        if(character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jumb_Force;
            player_Footsteps.in_Air = true;
        }
    }
}
