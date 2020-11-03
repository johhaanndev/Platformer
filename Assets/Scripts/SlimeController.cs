using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;

    private Animator anim;

    public float speed = 2f;
    private bool facingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (facingRight)
        {
            if (transform.position.x > posA.position.x)
            {
                transform.localScale = new Vector2(1, 1);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                facingRight = false;
            }
        }
        else
        {
            if (transform.position.x < posB.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                facingRight = true;
            }
        }
    }

    // When 2 slimes collide, they change directions
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Debug.Log("Collided with other slime"); Manual 
            
            if (facingRight)
            {
                facingRight = false;
            }
            else
            {
                facingRight = true;
            }
        }
    }

    public void JumpedOn()
    {
        anim.SetTrigger("JumpedOn");
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}