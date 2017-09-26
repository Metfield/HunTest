using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GeneralObject
{
    GameObject gameObject;
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    // Collision-related stuff
    Collider2D[] colliderResults;
    ContactFilter2D colliderContactFilter;

	public Projectile(Main inMain, GameObject go, Transform parentTransform)
    {
        // Set general variables
        SetGeneralVars(inMain, (int)go.transform.position.x, (int)go.transform.position.y);

        // Instantiate bullet object
        gameObject = GameObject.Instantiate(go);

        // Create rigid body
        rigidBody = gameObject.AddComponent<Rigidbody2D>();
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;        
        rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        // Create box collider
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        // Get SpriteRenderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Set parent (level)
        gameObject.transform.parent = parentTransform;

        // Make this ready for pooling
        gameObject.SetActive(false);

        // Setup collider related stuff
        // Will only care about layer 8 (Character)
        // This needs to be setup on every character game object
        colliderContactFilter.layerMask = LayerMask.NameToLayer("Character");
    }

    public void Prepare()
    {
        gameObject.SetActive(true);
    }

    public void Fire(Vector2 position, float velocity)
    {
        // Rotate accordingly
        if (velocity < 0)
            spriteRenderer.flipX = true;

        gameObject.transform.position = position;
        rigidBody.AddForce(new Vector2(velocity, 0), ForceMode2D.Impulse);
    }

    public bool IsAvailable()
    {
        return !gameObject.activeSelf;
    }

    private void Recycle()
    {
        spriteRenderer.flipX = false;
        rigidBody.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    public override bool FrameEvent()
    {
        if (!gameObject.activeSelf)
            return true;

        // Check if bullet has flown out of the viewport
        if (CheckOutOfBounds())
            return true;

        CheckForCollisions();

        return true;
    }

    private bool CheckOutOfBounds()
    {
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        // Check if it's no longer visible
        if (viewportPoint.x > 1 || viewportPoint.x < 0)
        {
            Recycle();
            return true;
        }

        return false;
    }

    private void CheckForCollisions()
    {
        // We only care about the first result
        // Need to create a new array every frame because Unity
        colliderResults = new Collider2D[1];

        // Make sure we're hitting a character
        if (boxCollider.OverlapCollider(colliderContactFilter, colliderResults) == 1)
        {
            // Specify from where is the target being hit
            // Note values are inverted
            int direction = rigidBody.velocity.x < 0 ? 1 : -1;

            // Explode projectile
            Explode();

            // It's an enemy
            if(colliderResults[0].gameObject.name.Contains("Enemy"))
            {
                game.EnemyIsGettingShot(colliderResults[0].gameObject.name, direction);
            }
            // It's the player
            else if(colliderResults[0].gameObject.name.Contains("Player"))
            {
                // Notify of target hit, specify from where
                game.PlayerIsGettingShot(direction);
            }

            // Sh shh, time to sleep little bullet
            Recycle();
        }
    }

    private void Explode()
    {
        // Kaboom FX!
    }
}
