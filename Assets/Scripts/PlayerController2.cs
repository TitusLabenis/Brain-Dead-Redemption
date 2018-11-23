using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

    private float moveSpeed = NORMALSPEED; //how fast the player moves

    //CONSTANT VALUES
    private const float NORMALSPEED = 20f;
    private const float SLOWSPEED = 10f;

    //checks if the player has his gun drawn
    private bool gunOut = false;

    private Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();//Fetches the animator
    }

    private void Update()
    {
        if (Input.GetButtonUp("j"))
        {
            DrawGun();
        }
    }

    //HANDLE PHYSICS HERE:
    void FixedUpdate()
    {

        //get input from the both axes and add forces
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveHorizontal, moveVertical)); //moves towards inputed direction

        Anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        Anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Slows down the player once it detects it moving into mud
        if (other.CompareTag("Slowdown"))
        {
            moveSpeed = SLOWSPEED;
            Debug.Log("Player is getting slowed down");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //resets player speed upon exiting the mud
        if (other.CompareTag("Slowdown"))
        {
            moveSpeed = NORMALSPEED;
            Debug.Log("Player has exited slowdown zone");
        }
    }

    private void DrawGun()
    {
        //TODO : Handle changing sprites and speed stuff here
    }
}
