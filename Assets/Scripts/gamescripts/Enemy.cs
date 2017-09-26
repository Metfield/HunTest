﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Character
{
    public Enemy(Main inMain, int health, int inX, int inY) : base(inMain, "Enemy", health, inX, inY)
    {        
        SetDirection(-1);
    }    

    public override bool FrameEvent()
    {
        // enemy logic here

        // If in sight and not dead
        if (isDead)
        {
            return isOK;
        }

        // temp logic :)
        //------------------------------------------------------------
        x = x + .4f*direction;
        if ((direction==1 && x > 600) || (direction==-1 && x < 480))
        {
            SetDirection(-direction);
        }
        //------------------------------------------------------------

        UpdatePos();

        return isOK;
    }

    public override void Kill(int hitDirection)
    {
        // Plays death animation
        base.Kill(hitDirection);

        // Ugly hack for time's sake
        // Push down corpse so it doesn't float on the air
        gameObject.transform.Translate(0, -15, 0);

        // Turn off collider
        boxCollider.enabled = false;

        // Play enemy death sound


    }

    public override void UpdatePos()
    {

        gfx.SetPos(gameObject, x, y);
    }

    void SetDirection(int inDirection)
    {
        direction = inDirection;

        //gfx.SetDirX(gameObject, direction);
        if (direction < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public override void Shoot()
    {
        throw new NotImplementedException();
    }

    public override void Turn(int direction)
    {
        throw new NotImplementedException();
    }

    public override void Jump()
    {
        throw new NotImplementedException();
    }
}