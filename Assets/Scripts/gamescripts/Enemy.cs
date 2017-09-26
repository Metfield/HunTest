using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Character
{
    Vector3 patrolVelocity;
    float patrolSpeed = 0.05f;

    Vector3 patrolLeftBound, patrolRightBound;

    public Enemy(Main inMain, int health, int inX, int inY) : base(inMain, "Enemy", health, inX, inY)
    {
        Init();
    }

    public Enemy(Main inMain, int health, GameObject go) : base(inMain, "Enemy", health, (int)go.transform.position.x, (int)go.transform.position.y, go)
    {
        Init();
    }

    private void Init()
    {
        Turn(-1);
        patrolVelocity = new Vector3(0, 0, 0);

        // Find patrol bounds
        patrolLeftBound = gameObject.transform.Find("PatrolLeftBound").transform.position;
        patrolRightBound = gameObject.transform.Find("PatrolRightBound").transform.position;

        // Object is kinematic
        rigidBody.isKinematic = true;
    }

    public override bool FrameEvent()
    {
        // If in sight and not dead
        if (isDead)
        {
            return isOK;
        }

        // Look


        // Shoot
        // Cooldown

        // Patrol
        Patrol();








        /*

          // temp logic :)
          //------------------------------------------------------------

          if ((direction==1 && x > 600) || (direction==-1 && x < 480))
          {
              Turn(-direction);
          }
          //------------------------------------------------------------
          */

        UpdatePos();

        return isOK;
    }

    void Patrol()
    {
        // Check bounds
        if (gameObject.transform.position.x <= patrolLeftBound.x)
        {
            Turn(-direction);
        }
        else if (gameObject.transform.position.x >= patrolRightBound.x)
        {
            Turn(-direction);
        }

        // Update velocity
        patrolVelocity.x = (direction * 2);
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
        rigidBody.transform.Translate(patrolVelocity);
    }

    public override void Shoot()
    {
        throw new NotImplementedException();
    }

    public override void Turn(int inDirection)
    {
        direction = inDirection;
        
        if (direction < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public override void Jump()
    {
        throw new NotImplementedException();
    }
}