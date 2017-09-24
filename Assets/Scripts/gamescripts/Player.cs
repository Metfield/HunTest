using System;
using UnityEngine;

public abstract class SpriteCharacter
{
    protected Main main;
    protected Game game;
    protected Gfx gfx;
    protected Snd snd;

    protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    protected GameObject gameObject;

    public abstract void UpdatePos();
    public abstract void Shoot();
    public abstract void Kill();
    public abstract void Turn(int direction);
    public abstract void Jump();
    public abstract void Duck();
    public abstract void StandStill();

    public virtual void FrameEvent() {  }
    public virtual void FrameEvent(int inMoveX, int inMoveY, bool inShoot) { }
}

public class Player : SpriteCharacter
{
    Vector3 playerPosition;

    float x, previousDx;
    float y;

    public Player (Main inMain, bool startFacingRight)
    {
        main = inMain;
        game = main.game;
        gfx  = main.gfx;
        snd  = main.snd;

        //Sprite[] sprites = gfx.GetLevelSprites("Players/Player1");

        x = 370;
        y = 624;

        // We're using the Unity engine, let's use prefabs :D
        if(!(gameObject = GameObject.Instantiate(GameObject.Find("Player"))))
        {
            main.Trace("Player constructor error. Make sure a Player is in the scene already");
            return;
        }

        gameObject.transform.parent = gfx.level.transform;
        gameObject.transform.position = new Vector3(x, -y, 1);

        playerPosition = gameObject.transform.localPosition;

        // Get key components
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    public override void FrameEvent(int inMoveX, int inMoveY, bool inShoot)
    {
        // Check if player is walking
        if (inMoveX == 0)
        {
            StandStill();
        }
        else
        {
            Walk(inMoveX);           
        }

        if (inMoveY == -1)
            Jump();

        if (inMoveY == 1)
            Duck();

        UpdatePos();

        if (inShoot)
        {
            Shoot();
        }
    }

    private void Walk(int dx)
    {
        // Flip sprite accordingly
        if (previousDx != dx)
            Turn(dx);

        // Update variables
        x = x + dx;
        previousDx = dx;

        // Play walk animation
        animator.SetBool("Walk", true);
    }

    public override void Jump()
    {
        main.Trace("Player::Jump!");
    }

    public override void Duck()
    {
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
        animator.SetBool("Duck", false);
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
    }
}