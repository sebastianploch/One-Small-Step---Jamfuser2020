﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayerController : MonoBehaviour
{
    [Range(0, 30f)]
    public float fSpeed;

    [Range(0, 15)]
    public float fMaxSpeed;
  
    public float fMaxJump;

    public float fJumpTimer;
    private bool bIsJumping;
    public LayerMask ground;

    public CameraShake cameraShake;

    private new Rigidbody2D rigidbody2D;
    private new Animator animation;
    private bool facingLeft;

    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        rigidbody2D.isKinematic = false;
        facingLeft = false;

        //fFallMultiplier = 2.5f;
        //fLowJumpMultiplier = 2.0f;
        fSpeed = 15f;
        fMaxSpeed = 7.59f;
        fMaxJump = 1.69f;
        rigidbody2D.gravityScale = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        movement = new Vector2(moveX, 0.0f);
        
        rigidbody2D.AddForce(movement * fSpeed * 5);
        rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, fMaxSpeed);
        animation.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            bIsJumping = true;
            StartCoroutine(cameraShake.Shake(0.35f, 0.05f));
        }

        if (moveX < 0 && !facingLeft)
        {
            reverseImage();
        }
        else if (moveX > 0 && facingLeft)
        {
            reverseImage();
        }

    }

    private void FixedUpdate()
    {
        if (bIsJumping && Input.GetButton("Jump"))
        {
            if (fJumpTimer < fMaxJump)
            {
                fJumpTimer += Time.deltaTime * 8;
                if (fJumpTimer > 0.6)
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, fJumpTimer * 5.0f);
            }
        }
        else
        {
            bIsJumping = false;
            fJumpTimer = 0.0f;
        }
    }

    public void HandleCollision(PlayerCollisionHandler playerCollisionHandler)
    {
        Debug.Log("You're colliding!");
    }

    private bool IsGrounded()
    {
        Vector2 pos = transform.position;
        Vector2 dir = Vector2.down;
        float dist = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, dist, ground);
        if (hit.collider != null)
            return true;
        return false;
    }

    private void reverseImage()
    {
        facingLeft = !facingLeft;
        Vector3 scale = rigidbody2D.transform.localScale;
        scale.x *= -1;
        rigidbody2D.transform.localScale = scale;
    }
}
