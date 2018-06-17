﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floormask;
    float camRayLength=100f;

    void Awake()
    {
        floormask = LayerMask.GetMask("Floor");
        anim=GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h,float v)
    {
        movement.Set(h, 0f, v);
        movement=movement.normalized*speed*Time.deltaTime; //Player moves at a constant speed in all the three planes
        playerRigidbody.MovePosition(transform.position + movement); //Apply the movement to the player
    }

    void Turning()
    {
        //Takes a point on the screen and casts a ray from that point into the scene
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floormask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);

        }
        
        
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

}
