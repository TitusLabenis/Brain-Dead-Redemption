using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //SPRITES FACING ALL FOUR DIRECTIONS
    [SerializeField]
    private Sprite cowboyFront;
    [SerializeField]
    private Sprite cowboyBack;
    [SerializeField]
    private Sprite cowboyLeft;
    [SerializeField]
    private Sprite cowboyRight;

    private SpriteRenderer spriteRenderer;

    //How quickly our character moves
    private float speed = NORMALSPEED;

    private const float NORMALSPEED = 15f;
    private const float SLOWSPEED = 5f;

    //Whether our player is moving or not
    private bool isMoving = true;

    //Player's animator
    private Animator pAnimator;

    private void Start()
    {
        //Access the components we need
        pAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //These determine the direction we are moving in
        float moveX = 0f;
        float moveY = 0f;
        
        //These change the direction we move in
        if (Input.GetKey(KeyCode.W))
        {
            FaceUp();
            moveY = +1f;
            pAnimator.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            FaceLeft();
            moveX = -1f;
            pAnimator.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            FaceDown();
            moveY = -1f;
            pAnimator.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            FaceRight();
            moveX = +1f;
            pAnimator.SetBool("isMoving", true);
        }

        //Slows down diagonal movement
        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        //Move the character in the specified direction
        transform.position += moveDir * speed * Time.deltaTime;

        if (moveX == 0 && moveY == 0)
        {
            //Checks if player is moving
            pAnimator.SetBool("isMoving", false);
        }

        //Shoot a raycast before moving to check for objects
        Vector3 targetMovePos = transform.position + moveDir * speed * Time.deltaTime;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime);
        if (raycastHit.collider == null)
        {
            //Nothing detected and our player can keep walking
            transform.position = targetMovePos;
            speed = NORMALSPEED;
        }

        else
        {
            //Stops character
            moveX = 0;
            moveY = 0;
            pAnimator.SetBool("isMoving", false);
            pAnimator.Play("Idle");
            
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slowdown"))
        {
            speed = SLOWSPEED;
            Debug.Log("Player is getting slowed down");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Slowdown"))
        {
            speed = NORMALSPEED;
            Debug.Log("Player has exited slowdown zone");
        }
    }

    private void FaceRight()
    {
        //Change the sprite if it isn't facing right currently
        if (spriteRenderer.sprite != cowboyRight)
        {
            spriteRenderer.sprite = cowboyRight;
            Debug.Log("=== after update, the sprite = " + spriteRenderer.sprite);
        }
    }

    private void FaceLeft()
    {
        //Change the sprite if it isn't facing left currently
        if (spriteRenderer.sprite != cowboyLeft)
        {
            Debug.Log("=== current sprite = " + spriteRenderer.sprite);
            spriteRenderer.sprite = cowboyLeft;
            Debug.Log("=== Changing Direction");
            Debug.Log("=== after update, the sprite = " + spriteRenderer.sprite);
        }
    }

    private void FaceUp()
    {
        //Change the sprite if it isn't facing up currently
        if (spriteRenderer.sprite != cowboyFront)
        {
            spriteRenderer.sprite = cowboyFront;
        }
    }

    private void FaceDown()
    {
        //Change the sprite if it isn't facing down currently
        if (spriteRenderer.sprite != cowboyBack)
        {
            spriteRenderer.sprite = cowboyBack;
        }
    }
}
