using System;
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


    public override void UpdatePos()
    {
        gfx.SetPos(gameObject, x, y);
    }

    void SetDirection(int inDirection)
    {
        direction = inDirection;
        gfx.SetDirX(gameObject, direction);
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