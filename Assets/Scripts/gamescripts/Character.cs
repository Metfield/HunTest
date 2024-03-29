﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : GeneralObject
{ 
    protected int health;

    protected Animator animator;

    protected float movementSpeed;
    protected bool isGrounded;

    protected SpriteRenderer spriteRenderer;
    protected GameObject gameObject;
    protected Rigidbody2D rigidBody;
    protected BoxCollider2D boxCollider;

    protected GameObject projectileOrigin;

    protected bool isDead;

    static uint uid = 0;

    TextMesh healthText;

    public Character(Main inMain, string characterGameObjectName, int health, int inX, int inY, GameObject go = null)
    {
        SetGeneralVars(inMain, inX, inY);
        isDead = false;

        // We're using the Unity engine, let's use prefabs :D
        if (go != null)
            // Get game object if we already have one
            gameObject = go;
        else
        {
            gameObject = GameObject.Instantiate(GameObject.Find(characterGameObjectName));
            gameObject.transform.parent = gfx.level.transform;
            gameObject.transform.position = new Vector3(x, -y, 1);
        }

        // Get key components
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();

        // Get projectile origin        
        projectileOrigin = gameObject.transform.Find("BulletOrigin").gameObject;

        // (Super) simple way of generating unique identifiers for character
        gameObject.name = characterGameObjectName + "_" + uid++;

        // Set initial health
        this.health = health;
        healthText = gameObject.transform.Find("HealthUI").GetComponent<TextMesh>();
        healthText.text = "HP " + health;
    }

    public abstract void UpdatePos();

    public virtual void Shoot()
    {
        // Play animation
        animator.SetTrigger("Shoot");

        // Play sound
        snd.PlayAudioClip("Gun");
    }

    public abstract void Turn(int direction);
    public abstract void Jump();
    public virtual void Melee() { }

    public virtual void Kill(int hitDirection)
    {
        // Character is dead :'(
        main.Trace(gameObject.name + " Dies!!");
        isDead = true;

        // Turn off health UI
        healthText.text = "";

        // Fall front or back
        // Character is facing left
        if (spriteRenderer.flipX)
        {
            // Fall to back
            if(hitDirection < 0)
            {
                animator.SetTrigger("DieBack");
            }
            // Fall to front
            else
            {
                animator.SetTrigger("DieFront");
            }
        }
        // Character is facing right
        else
        {
            // Fall to front
            if (hitDirection < 0)
            {
                animator.SetTrigger("DieFront");
            }
            // Fall to back
            else
            {
                animator.SetTrigger("DieBack");
            }
        }

        // Ugly hack for time's sake
        // Push down corpse so it doesn't float on the air
        gameObject.transform.Translate(0, -15, 0);
        rigidBody.velocity = Vector2.zero;
        rigidBody.isKinematic = true;
        boxCollider.enabled = false;
    }

    public virtual void OnBeingShot(int hitDirection, Projectile projectile = null)
    {
        //main.Trace(gameObject.name + " takes Damage, " + --health + " HP left");

        if (--health == 0)
        {
            Kill(hitDirection);
            return;
        }

        // Update health UI
        healthText.text = "HP " + health;

        // Play hit sound
        snd.PlayAudioClip("Grunt");

        // Some blood FX?

    }

    public void Duck()
    {
        animator.SetBool("Duck", true);
    }
        
    public void StandUp()
    {
        animator.SetBool("Duck", false);
    }

    public void StandStill()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Jump", false);
    }

    public string GetName()
    {
        return gameObject.name;
    }
}

