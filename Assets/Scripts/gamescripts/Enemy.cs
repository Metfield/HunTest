﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Character
{
    Vector3 patrolVelocity;
    float patrolSpeed = 2f;

    Vector3 patrolLeftBound, patrolRightBound;

    float sightRange = 400;
    
    // Shooting-related variables
    bool isShooting;
    int shotsPerBurst, shotsLeft;
    float burstLengthInSecs;
    float burstStep;
    float burstStart;
    float dt;

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

        // Initialize shooting-related stuff
        shotsPerBurst = 3;
        burstLengthInSecs = 0.8f;
        burstStep = burstLengthInSecs / shotsPerBurst;
    }

    public override bool FrameEvent()
    {
        // If in sight and not dead
        if (isDead)
        {
            return isOK;
        }

        if(isShooting)
        {
            Shoot();
        }
        else
        {
            // Patrol
            Patrol();

            // Look
            Look();

            // Update position
            UpdatePos();
        }

        return isOK;
    }

    void Look()
    {
        //Debug.DrawRay(projectileOrigin.transform.position, new Vector2(direction, 0) * sightRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(projectileOrigin.transform.position, new Vector2(direction, 0), sightRange);

        if (hit.transform != null)
        {
            // Check if hit was player
            if (hit.transform.gameObject.name.Contains("Player"))
            {
                // Target spotted!
                isShooting = true;
                main.Trace(GetName() + " sees " + hit.transform.gameObject.name);

                // Set time for start of shooting burst
                burstStart = Time.realtimeSinceStartup;
                shotsLeft = shotsPerBurst;
            }
        }
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
        patrolVelocity.x = (direction * patrolSpeed);
    }

    public override void Shoot()
    {
        dt = Time.realtimeSinceStartup;

        if (dt - burstStart > burstStep)
        {
            // Update stamp
            burstStart = dt;

            // Handle animation, sound and actual projectile
            base.Shoot();
            gfx.FireProjectile(projectileOrigin.transform.position, direction);

            if (--shotsLeft == 0)
                isShooting = false;
        }
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

    public override void Turn(int inDirection)
    {
        direction = inDirection;
        projectileOrigin.transform.localPosition = new Vector3(-1 * projectileOrigin.transform.localPosition.x, projectileOrigin.transform.localPosition.y, projectileOrigin.transform.localPosition.z);

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