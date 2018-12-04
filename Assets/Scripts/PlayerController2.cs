using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour {


    //Is the player idle
    private bool canShoot = false;
    //How many kills the player has
    private float score = 0f;

    //Maximum and current ammo
    private float currentAmmo = 6f;
    private float maxAmmo = 6f;

    private float moveSpeed = NORMALSPEED; //how fast the player moves

    private float moveHorizontal = 0f;
    private float moveVertical = 0f;

    [SerializeField]
    private Text scoreText;

    //REVOLVER UI SPRITES:
    [SerializeField]
    private Image Revolver6;
    [SerializeField]
    private Image Revolver5;
    [SerializeField]
    private Image Revolver4;
    [SerializeField]
    private Image Revolver3;
    [SerializeField]
    private Image Revolver2;
    [SerializeField]
    private Image Revolver1;
    [SerializeField]
    private Image Revolver0;

    //SHOOT ORIGINS
    [SerializeField]
    private Transform leftOrigin;
    [SerializeField]
    private Transform rightOrigin;
    [SerializeField]
    private Transform upOrigin;
    [SerializeField]
    private Transform downOrigin;

    [SerializeField]
    private Transform bulletTrailPrefab;

    private LineRenderer gunTrail;
    private AudioSource playerAudio;

    //CONSTANT VALUES
    //speeds
    private const float NORMALSPEED = 15f;
    private const float SLOWSPEED = 2f;
    private const float VERYSLOWSPEED = 1f;

    //PLAYER FACING DIRECTIONS
    private const float UP = 0f;
    private const float DOWN = 1f;
    private const float LEFT = 2f;
    private const float RIGHT = 3f;

    public float moveDir;

    //checks if the player has his gun drawn
    private bool gunOut = false;

    private Animator Anim;


    private void Start() //FETCHING COMPONENTS
    {
        Anim = GetComponent<Animator>();//Controls the animations
        gunTrail = GetComponent<LineRenderer>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        scoreText.text = "Score : " + score;

        if (currentAmmo == 0)
        {
            Revolver0.enabled = true;
            Revolver1.enabled = false;
        }

        if (currentAmmo == 1)
        {
            Revolver1.enabled = true;
            Revolver0.enabled = false;
            Revolver2.enabled = false;
        }

        if (currentAmmo == 2)
        {
            Revolver2.enabled = true;
            Revolver1.enabled = false;
            Revolver3.enabled = false;
        }

        if (currentAmmo == 3)
        {
            Revolver3.enabled = true;
            Revolver2.enabled = false;
            Revolver4.enabled = false;
        }

        if (currentAmmo == 4)
        {
            Revolver4.enabled = true;
            Revolver3.enabled = false;
            Revolver5.enabled = false;
        }

        if (currentAmmo == 5)
        {
            Revolver5.enabled = true;
            Revolver4.enabled = false;
            Revolver6.enabled = false;
        }

        if (currentAmmo == 6)
        {
            Revolver6.enabled = true;
            Revolver5.enabled = false;
        }

        if (Input.GetButtonUp("j")) //DRAWING GUN
        {
            if (gunOut == false)
            {
                DrawGun();
                Debug.Log("Player Drew Gun");
            }

            else if (gunOut == true)
            {
                HolsterGun();
                Debug.Log("Player Holstered Gun");
            }
        }

        //MOVE DIRECTION
        if (Input.GetAxisRaw("Horizontal") == 1f)
        {
            moveDir = RIGHT;
        }

        else if (Input.GetAxisRaw("Horizontal") == -1f)
        {
            moveDir = LEFT;
        }

        else if (Input.GetAxisRaw("Vertical") == 1f)
        {
            moveDir = UP;
        }

        else if (Input.GetAxisRaw("Vertical") == -1f)
        {
            moveDir = DOWN;
        }

        //will shoot to the right if the player is moving diagonally
        else if (Input.GetAxisRaw("Vertical") != 0f && Input.GetAxisRaw("Horizontal") == 1f)
        {
            moveDir = RIGHT;
        }

        else if (Input.GetAxisRaw("Vertical") != 0f && Input.GetAxisRaw("Horizontal") == -1f)
        {
            moveDir = LEFT;
        }

        //SHOOTING:
        if (Input.GetButtonUp("shoot") && gunOut == true && currentAmmo >= 1 && canShoot == true)
        {
            Shoot();
        }

        //RELOADING:
        if (Input.GetButtonUp("reload") && currentAmmo != maxAmmo && canShoot == false && gunOut == true)
        {
            currentAmmo = currentAmmo + 1;
        }
    }

    //HANDLE PHYSICS HERE:
    void FixedUpdate()
    {

        //get input from the both axes and add forces
        moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (moveHorizontal == 0f)
        {
            moveHorizontal = 0;
            moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveHorizontal, moveVertical));
            Anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
            Anim.SetFloat("MoveX", 0);
        }

        else if (moveHorizontal != 0f)
        {
            Anim.SetFloat("MoveY", 0);
        }

        transform.Translate(new Vector3(moveHorizontal, 0)); //moves towards inputed direction

        Anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));

        if (Anim.GetFloat("MoveX") == 0 && Anim.GetFloat("MoveY") == 0)
        {
            canShoot = false;
        }

        else if (Anim.GetFloat("MoveX") != 0 || Anim.GetFloat("MoveY") != 0)
        {
            canShoot = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Slows down the player once it detects it moving into mud
        if (other.CompareTag("Slowdown"))
        {
            if (gunOut == false)
            {
                moveSpeed = SLOWSPEED;
                Debug.Log("Player is getting slowed down");
            }
            
            if (gunOut == true)
            {
                moveSpeed = VERYSLOWSPEED;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //resets player speed upon exiting the mud
        if (other.CompareTag("Slowdown"))
        {
            if (gunOut == false)
            {
                moveSpeed = NORMALSPEED;
                Debug.Log("Player has exited slowdown zone");
            }

            if (gunOut == true)
            {
                moveSpeed = SLOWSPEED;
                Debug.Log("Player has exited slowdown zone");
            }
        }
    }

    private void DrawGun()
    {
        gunOut = true; //tell the engine the player has drawn the gun
        Anim.SetBool("GunDrawn", true); //relay the message to the animator
        moveSpeed = SLOWSPEED; //slow down the player
    }

    private void HolsterGun()
    {
        gunOut = false; //tell the engine that the gun is now holstered
        Anim.SetBool("GunDrawn", false); //relay the message to the animator
        moveSpeed = NORMALSPEED; //restore the movement speed
    }

    private void Shoot()
    {
        DrawTrail();
        currentAmmo = currentAmmo - 1;

        Debug.Log("Player Shot Bullet");
        if (moveDir == RIGHT)
        {
            RaycastHit2D hit = Physics2D.Raycast(rightOrigin.position, Vector2.right);
            Debug.DrawRay(rightOrigin.position, Vector2.right * 50, Color.red); //Draws a line only viewable on scene
            if (hit.collider.CompareTag("Zombie"))
            {
                hit.transform.SendMessage("HitByPlayer"); //sends a message to the zombie's script when hit
                score = score + 1;
            }
        }

        else if (moveDir == LEFT)
        {
            RaycastHit2D hit = Physics2D.Raycast(leftOrigin.position, Vector2.left);
            Debug.DrawRay(leftOrigin.position, Vector2.left * 50, Color.red); //Draws a line only viewable on scene
            if (hit.collider.CompareTag("Zombie"))
            {
                hit.transform.SendMessage("HitByPlayer"); //sends a message to the zombie's script when hit
                score = score + 1;
            }
        }

        else if (moveDir == UP)
        {
            RaycastHit2D hit = Physics2D.Raycast(upOrigin.position, Vector2.up);
            Debug.DrawRay(upOrigin.position, Vector2.up * 50, Color.red); //Draws a line only viewable on scene
            if (hit.collider.CompareTag("Zombie"))
            {
                hit.transform.SendMessage("HitByPlayer"); //sends a message to the zombie's script when hit
                score = score + 1;
            }
        }

        else if (moveDir == DOWN)
        {
            RaycastHit2D hit = Physics2D.Raycast(downOrigin.position, Vector2.down);
            Debug.DrawRay(downOrigin.position, Vector2.down * 50, Color.red); //Draws a line only viewable on scene
            if (hit.collider.CompareTag("Zombie"))
            {
                hit.transform.SendMessage("HitByPlayer"); //sends a message to the zombie's script when hit
                score = score + 1;
            }
        }
    }

    private void DrawTrail()
    {
        if (moveDir == LEFT)
        {
            Instantiate(bulletTrailPrefab, leftOrigin.position, Quaternion.Euler(0,180,0));
        }

        else if (moveDir == RIGHT)
        {
            Instantiate(bulletTrailPrefab, rightOrigin.position, Quaternion.Euler(0, 0, 0));
        }

        else if (moveDir == UP)
        {
            Instantiate(bulletTrailPrefab, upOrigin.position, Quaternion.Euler(0, 0, 90));
        }

        else if (moveDir == DOWN)
        {
            Instantiate(bulletTrailPrefab, downOrigin.position, Quaternion.Euler(0,0,-90));
        }
    }
}
