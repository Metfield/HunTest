using System;
using UnityEngine;

public abstract class SpriteCharacter
{
    protected Main main;
    protected Game game;
    protected Gfx gfx;
    protected Snd snd;

    protected Animator animator;

    protected float movementSpeed;
    protected bool isGrounded;

    protected SpriteRenderer spriteRenderer;
    protected GameObject gameObject;
    protected Rigidbody2D rigidBody;

    public abstract void UpdatePos();
    public abstract void Shoot();
    public abstract void Kill();
    public abstract void Turn(int direction);
    public abstract void Jump();
    public abstract void Duck();
    public abstract void StandUp();
    public abstract void StandStill();

    public virtual void FrameEvent() {  }
    public virtual void FrameEvent(int inMoveX, int inMoveY, bool inShoot) { }
}

public class Player : SpriteCharacter
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

    public Player (Main inMain)
    {
        main = inMain;
        game = main.game;
        gfx  = main.gfx;
        snd  = main.snd;

        //Sprite[] sprites = gfx.GetLevelSprites("Players/Player1");

        x = 370;
        y = 624;
        movementSpeed = 3.0f;

        // We're using the Unity engine, let's use prefabs :D
        gameObject = GameObject.Instantiate(GameObject.Find("Player"));
        gameObject.transform.parent = gfx.level.transform;
        gameObject.transform.position = new Vector3(x, -y, 1);

        playerPosition = gameObject.transform.localPosition;

        // Get key components
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        // Get distance from collider to bottom
        colliderExtents = gameObject.GetComponent<BoxCollider2D>().bounds.extents;

        // Initialiaze jumping-related variables
        groundedCastMargin = 0.2f;
        horizontalExtents = new Vector3(colliderExtents.x, 0, 0);
        heightOffset = new Vector3(0.0f, -0.01f);

        // Set initial position in map
        UpdatePos();
    }

    public override void FrameEvent(int inMoveX, int inMoveY, bool inShoot)
    {
        CheckPlayerIsGrounded();

        if (isGrounded)
            GameObject.Find("Blah").GetComponent<SpriteRenderer>().enabled = true;
        else
            GameObject.Find("Blah").GetComponent<SpriteRenderer>().enabled = false;

        if (inMoveY == -1 && isGrounded)
            Jump();
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

        if (inShoot)
        {
            Shoot();
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
        rigidBody.AddForce(new Vector2(dx * movementSpeed * (isGrounded ? physicsCompensationMultiplier : 1), 0), ForceMode2D.Impulse);

        // Play walk animation
        animator.SetBool("Walk", true);        
    }

    public override void Jump()
    {
        if (isGrounded)
        {
            // Play jump animation
            animator.SetTrigger("Jump");
        
            main.Trace("Player::Jump!");
            
            rigidBody.AddForce(gameObject.transform.up * physicsCompensationMultiplier * 10000);
        }
        else
        {

        }
    }

    public override void Duck()
    {
        animator.SetBool("Duck", true);
        main.Trace("Player::Duck!"); 
    }

    public override void UpdatePos()
    {
        playerPosition.x = x;
        playerPosition.y = -y;        

        gameObject.transform.localPosition = playerPosition;
    }

    public override void Kill()
    {
        
    }

    public override void StandStill()
    {        
        animator.SetBool("Walk", false);        
    }

    public override void Turn(int direction)
    {
        if (previousDx > direction)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public override void Shoot()
    {
        // Play animation
        animator.SetTrigger("Shoot");

        // Play sound
        snd.PlayAudioClip("Gun");

        //Spawn projectile
    }

    public override void StandUp()
    {
        animator.SetBool("Duck", false);
    }
}