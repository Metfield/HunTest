using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GeneralObject
{
    GameObject gameObject;
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

	public Projectile(GameObject go, Transform parentTransform)
    {
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

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        // Check if it's no longer visible
        if (viewportPoint.x > 1 || viewportPoint.x < 0)
            Recycle();

        return true;
    }
}
