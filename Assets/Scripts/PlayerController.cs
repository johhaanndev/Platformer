using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private AudioSource jumpClip;
    [SerializeField] private AudioSource coinClip;

    // collider variables
    private Collider2D coll;
    [SerializeField] private LayerMask ground;

    // movement parameters
    public float speed = 5f;
    public float playerJumpForce = 10f;
    public float killJumpForce = 5f;

    // gameplay parameters
    private bool isWin = false;
    private bool isDead = false;
    private int points = 0;
    private int coinPoints = 50;


    // Finite State Machine declaration
    private enum State { idle, run, jump, fall, death};
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("timer: " + timer);
        // Movement only when player is alive
        if (state != State.death && !isWin)
        {
            Movement();
        }
        StateSwitch();
        anim.SetInteger("state", (int)state);
    }

    // method to create the basic movement by key inputs
    private void Movement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        // jump - GetKeyDown to just play on the moment the key is pressed
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && coll.IsTouchingLayers(ground)) // added spacebar because some players may feel more comfortable
        {
            Jump(playerJumpForce);
        }
        
    }

    // created jump in a method this way we can use it when enemy is killed
    private void Jump(float jumpForce)
    {
        jumpSound();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump; // jump state activate when jump button is pressed
    }

    // method that manages the FSM for a well animator use
    private void StateSwitch()
    {
        if (isDead == true)
        {
            state = State.death;
            // Debug.Log("dead"); Manual debug
        }
        else
        {
            // when jump, check y velocity for falling animation
            if (state == State.jump)
            {
                if (rb.velocity.y < 0f)
                {
                    state = State.fall;
                }
            }
            // player can fall off of the platform, fall animation activate
            else if (!coll.IsTouchingLayers(ground) && rb.velocity.y < 0f)
            {
                state = State.fall;
            }
            // when hits the floor, switch to idle animation
            else if (state == State.fall)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    state = State.idle;
                }
            }
            // if velocity x is greater than 0.2, switch to run animation
            else if (Mathf.Abs(rb.velocity.x) > 0.5f)
            {
                state = State.run;
            }
            // when stopped, switch to idle animation
            else if (Mathf.Abs(rb.velocity.x) < 0.5f)
            {
                state = State.idle;
            }
        }
        
    }

    // when player hits the enemy
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // if the player jumped on it, enemy is killed
            if (state == State.fall)
            {
                SlimeController slime = other.gameObject.GetComponent<SlimeController>();
                slime.JumpedOn();
                Jump(killJumpForce);
                points += 100;
            }
            // if the player didnt jump on it, player dies
            else
            {
                isDead = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            coinClip.Play();
            points += 50;
            Destroy(other.gameObject);
            
        }
        // player win
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            isWin = true;
        }
    }

    public bool getWin()
    {
        return isWin;
    }

    public bool getDead()
    {
        return isDead;
    }

    public int getPoints()
    {
        return points;
    }


    private void jumpSound()
    {
        jumpClip.Play();
    }

    // When player dies, menu pops up
    private void Death()
    {
        gameObject.SetActive(false);
        gameplayUI.SetActive(false);
        deathMenu.SetActive(true);
    }
}
