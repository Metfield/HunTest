using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool
{
    // Had to expose this to add them to game objects in Main :(
    public List<Projectile> projectiles;

    GameObject projectileGameObject;
    Transform parentTransform;
    Main main;
	
	public ProjectilePool(int initialSize, Main inMain)
    {
        main = inMain;

        // Save parent transform
        parentTransform = main.gfx.level.transform;

        // Initialize pool with specified size
        projectiles = new List<Projectile>(initialSize);

        // Create one projectile object to clone
        projectileGameObject = main.gfx.MakeGameObject("Projectile", Resources.Load<Sprite>("Weapons/Bullets/Bullet"));
        projectileGameObject.transform.localScale = new Vector3(100f, 100f, 1);

        // Create objects and add to object list
        for (int i = 0; i < initialSize; i++)
        {
            projectiles.Add(new Projectile(main, projectileGameObject, parentTransform));
        }

        projectileGameObject.SetActive(false);
    }

    public Projectile GetProjectile()
    {
        foreach(Projectile projectile in projectiles)
        {
            if (projectile.IsAvailable())
            {
                projectile.Prepare();
                return projectile;
            }
        }

        Projectile newProjectile = new Projectile(main, projectileGameObject, parentTransform);
        newProjectile.Prepare();
        projectiles.Add(newProjectile);

        return newProjectile;
    }

    public int GetSize()
    {
        return projectiles.Count;
    }
}
