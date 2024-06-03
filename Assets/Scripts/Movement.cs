using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpPower = 10;
    public bool booolean = true;
    public LayerMask groundLayer;
    public bool spriteFlipped;
    public SpriteRenderer renderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.flipX = spriteFlipped;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        booolean = hit.collider;

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && booolean)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            spriteFlipped = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            spriteFlipped = true;
        }

    }


}
