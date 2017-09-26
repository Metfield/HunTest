using System.Collections;
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

    static uint uid = 0;

    public Character(Main inMain, string characterGameObjectName, int health, int inX, int inY)
    {
        SetGeneralVars(inMain, inX, inY);

        // We're using the Unity engine, let's use prefabs :D
        gameObject = GameObject.Instantiate(GameObject.Find(characterGameObjectName));
        gameObject.transform.parent = gfx.level.transform;
        gameObject.transform.position = new Vector3(x, -y, 1);

        // Get key components
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        // Get projectile origin        
        projectileOrigin = gameObject.transform.Find("BulletOrigin").gameObject;

        // (Super) simple way of generating unique identifiers for character
        gameObject.name = characterGameObjectName + "_" + uid++;

        // Set initial health
        this.health = health;
    }

    public abstract void UpdatePos();
    public abstract void Shoot();
    public abstract void Turn(int direction);
    public abstract void Jump();

    public virtual void Kill()
    {
        main.Trace(gameObject.name + " Dies!!");
        gameObject.SetActive(false);

        // Fall front or back
//        if(rigidBody.velocity < 0)

    }

    public void OnBeingShot()
    {
        main.Trace(gameObject.name + " takes Damage, " + --health + " HP left");

        if (health == 0)
        {
            Kill();
        }
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

