using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    Vector3 playerPosition;

    float x, previousDx;
    float y;

    // Gotta tickle Unity with these helpers in order for jumping not to go insane
    float groundedCastMargin;
    Vector3 horizontalExtents;
    Vector3 heightOffset;

    bool isCrouching;
    float physicsCompensationMultiplier = 4000;
    Vector3 colliderExtents;
        
    GameObject walkingGun;

    Vector3 viewportCoordinates;
    float cameraTrackingBounds; 

    public Player (Main inMain, int initialHealth) : base(inMain, "Player", initialHealth, 370, 624)
    {
        movementSpeed = 3.0f;
        x = 370;
        y = 624;

        // Assuming we are starting facing right
        previousDx = 1;        

        playerPosition = gameObject.transform.localPosition;
      
        // Enable renderer
        spriteRenderer.enabled = true;

        // Get distance from collider to bottom
        colliderExtents = gameObject.GetComponent<BoxCollider2D>().bounds.extents;

        // Initialiaze jumping-related variables
        groundedCastMargin = 0.2f;
        horizontalExtents = new Vector3(colliderExtents.x, 0, 0);
        heightOffset = new Vector3(0.0f, -0.01f);

        // Get walking gun
        walkingGun = gameObject.transform.Find("WalkingGun").gameObject;
        walkingGun.SetActive(false);
       
        // Set initial position in map
        UpdatePos();

        // Follow player with camera when beyond 70% of the viewport
        cameraTrackingBounds = 0.7f;
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inShoot)
    {
        CheckPlayerIsGrounded();

        if (inMoveX == 0)
            walkingGun.SetActive(false);

        // Lets player jump through platform
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Level"), !isGrounded && rigidBody.velocity.y > 0);

        if (isGrounded)
        {
            if (inMoveY == -1)
            { 
                Jump();
            }
            else if (inMoveY == 1)
            {
                isCrouching = true;
                StandStill();
                Duck();
            }
            else
            {
                StandUp();
                isCrouching = false;
            }
        }

        // Check if player is walking
        if (inMoveX == 0)
        {
            StandStill();
        }
        else
        {
            if(!isCrouching)
                Walk(inMoveX);           
        }

        if(!isGrounded)
            animator.SetBool("Jump", true);

        if (inShoot)
            Shoot();

        // Handle camera movement
        TraceCamera();
    }

    private void TraceCamera()
    {
        // Get viewport coordinates
        viewportCoordinates = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        
        // Move camera only if we're nearing edge of screen
        // It failed as it was jumping around too muchg... needed more work but there's no time!
        //if(viewportCoordinates.x < 1 - cameraTrackingBounds || viewportCoordinates.x > cameraTrackingBounds)
        {
            // Only care for X right now
            gfx.MoveCamera(new Vector3(gameObject.transform.position.x, gfx.cam.transform.position.y, gfx.cam.transform.position.z));
        }
    }

    private void CheckPlayerIsGrounded()
    {
        isGrounded = Physics2D.Raycast(gameObject.transform.position + heightOffset - horizontalExtents, -gameObject.transform.up, groundedCastMargin).collider != null
                     || Physics2D.Raycast(gameObject.transform.position + heightOffset, -gameObject.transform.up, groundedCastMargin ).collider != null
                     || Physics2D.Raycast(gameObject.transform.position + heightOffset + horizontalExtents, -gameObject.transform.up, groundedCastMargin).collider != null;
    }

    private void Walk(int dx)
    {
        // Flip sprite accordingly
        if (previousDx != dx)
            Turn(dx);

        // Update variables
        x = x + dx;
        previousDx = dx;

        // Add force to player
        rigidBody.AddForce(new Vector2(dx * movementSpeed * (isGrounded ? physicsCompensationMultiplier : physicsCompensationMultiplier / 2), 0), ForceMode2D.Impulse);

        // Play walk animation
        if (isGrounded)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Jump", false);
        }
    }

    bool jumpCooldown = false;
    float jumpStartTime;

    public override void Jump()
    {
        if (!jumpCooldown)
        {
            // Play jump animation
            animator.SetBool("Jump", true);
            main.Trace("Player::Jump!");
            rigidBody.AddForce(gameObject.transform.up * physicsCompensationMultiplier * 100, ForceMode2D.Impulse);

            jumpCooldown = true;
            jumpStartTime = 0;
        }
        else
        {
            // Wait for 0.2 secs after jump to avoid stupid bouncing
            if((jumpStartTime += Time.deltaTime) > 0.02f)
            {
                jumpCooldown = false;
            }
        }
    }

    public override void UpdatePos()
    {
        playerPosition.x = x;
        playerPosition.y = -y;        

        gameObject.transform.localPosition = playerPosition;
    }

    /*public override void Kill()
    {
        
    }*/

    public override void Turn(int direction)
    {
        if (previousDx > direction)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public override void Shoot()
    {
        if (isCrouching)
            return;

        // Play animation
        animator.SetTrigger("Shoot");

        // Play sound
        snd.PlayAudioClip("Gun");

        //Spawn projectile
        Vector3 origin = projectileOrigin.transform.position;

        if (previousDx < 0)
            // TODO: Find nicer way... get localToWorld coordinates somehow
            origin -= new Vector3(projectileOrigin.transform.localPosition.x * 6, 0, 0);

        // Fire!!
        gfx.FireProjectile(origin, previousDx);

        // Spawn extra gun if player is walking'n'shootin'
        if (isGrounded)
        {
            if (PlayerIsMovingHorizontally())
            {
                UpdateWalkingGun();
                walkingGun.SetActive(true);
            }
            else
            {
                walkingGun.SetActive(false);
            }
        }
    }

    bool PlayerIsMovingHorizontally()
    {
        return rigidBody.velocity.x != 0;
    }

    void UpdateWalkingGun()
    {
        // Update position and mirroring accordingly
        // only if player changed orientation
        if (previousDx > 0)
        {
            if (walkingGun.transform.localPosition.x < 0)
            {
                walkingGun.transform.localPosition -= new Vector3(walkingGun.transform.localPosition.x * 2, 0, 0);
                walkingGun.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else if (previousDx < 0)
        {
            if (walkingGun.transform.localPosition.x > 0)
            {
                walkingGun.transform.localPosition -= new Vector3(walkingGun.transform.localPosition.x * 2, 0, 0);
                walkingGun.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}