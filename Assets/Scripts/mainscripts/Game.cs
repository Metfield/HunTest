using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    Main main;
    int myRes;
    Gfx gfx;
    Snd snd;

    static string PLAY = "play";

    string gameStatus;

    int   camWidth;
    int   camHeight;
    float camX;
    float camY;

    Player player;

    bool leftKey, rightKey, jumpKey, duckKey, shootKey;
    int playerHorizontal, playerVertical;
    bool playerShoot;
    bool playerShootRelease = true;

    bool meleeKey, melee;

    List<Enemy> enemyObjects;
    List<GeneralObject> gameObjects;

    int gameObjectLength;

    public void Init(Main inMain)
    {
        main  = inMain;
        gfx   = main.gfx;
        myRes = gfx.myRes;
        snd   = main.snd;

        camWidth  = gfx.screenWidth / myRes;
        camHeight = gfx.screenHeight / myRes;

        gameObjects = new List<GeneralObject>();
        gameObjectLength = 0;

        enemyObjects = new List<Enemy>();

        player = new Player(main, 5);

        FindEnemiesInScene();

        // Create projectile pool
        gfx.StartProjectiles();

        // Add projectiles to object list
        foreach(Projectile p in gfx.GetProjectilePool().projectiles)
        {
            AddLevelObject(p);
        }

        gameStatus  = PLAY;

        // Start music
        snd.PlayBGM();
    }

    void Update()
    {
        if (gameStatus==PLAY)
        {
            GoKeys();

            GoPlayer();

            GoCam();

            GoObjects();
        } 
    }

    void GoPlayer()
    {
        player.FrameEvent(playerHorizontal, playerVertical, playerShoot, melee);
    }

    private void GoKeys()
    {
        // ---------------------------------------------------------------
        // NORMAL KEYBOARD
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.LeftArrow))  { leftKey   = true; }
        if (Input.GetKeyUp(KeyCode.LeftArrow))    { leftKey   = false; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { rightKey  = true; }
        if (Input.GetKeyUp(KeyCode.RightArrow))   { rightKey  = false; }
        if (Input.GetKeyDown(KeyCode.UpArrow))    { jumpKey   = true; }
        if (Input.GetKeyUp(KeyCode.UpArrow))      { jumpKey   = false; }
        if (Input.GetKeyDown(KeyCode.DownArrow))  { duckKey   = true; }
        if (Input.GetKeyUp(KeyCode.DownArrow))    { duckKey   = false; }

        if (Input.GetKeyDown(KeyCode.Z))          { jumpKey   = true;  }
        if (Input.GetKeyUp(KeyCode.Z))            { jumpKey   = false; }
        if (Input.GetKeyDown(KeyCode.A))          { shootKey  = true; }
        if (Input.GetKeyUp(KeyCode.A))            { shootKey  = false; }

        if (Input.GetKeyDown(KeyCode.Q)) { meleeKey = true; }
        if (Input.GetKeyUp(KeyCode.Q)) { meleeKey = false; }

        if (Input.GetKeyDown(KeyCode.K)) { player.Kill(1); }

        playerHorizontal = 0;
        if (leftKey) { playerHorizontal-=1; }
        if (rightKey) { playerHorizontal+=1; }

        playerVertical = 0;
        if (jumpKey) { playerVertical-=1; }
        if (duckKey) { playerVertical+=1; }

        melee = meleeKey;
        meleeKey = false;

        jumpKey = false;
        playerShoot = false;

        if (shootKey)
        {
            if (playerShootRelease)
            {
                playerShootRelease = false;
                playerShoot = true;
            }
        }
        else
        {
            if (!playerShootRelease)
            {
                playerShootRelease = true;
            }
        }
    }
    
    void GoCam()
    {
        camX = 480 - camWidth/2;
        camY = 600 - camHeight/2;

        gfx.MoveLevel(camX, camY);
    }

    public void AddLevelObject(GeneralObject inObj)
    {
        gameObjects.Add(inObj);
        gameObjectLength++;
    }

    public void AddLevelEnemy(Enemy enemy)
    {
        enemyObjects.Add(enemy);
    }

    void FindEnemiesInScene()
    {
        foreach(GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            AddLevelEnemy(new Enemy(main, 3, enemyObject));
        }
    }

    public void EnemyIsGettingShot(string enemyName, int hitDirection)
    {
        main.Trace(enemyName + " is getting shot");

        // Search could be done in parallel
        foreach(Enemy enemy in enemyObjects)
        {
            if(enemy.GetName() == enemyName)
            {
                enemy.OnBeingShot(hitDirection);
                break;
            }
        }
    }

    // Assuming 1P, otherwise just need to perform super simple search
    public void PlayerIsGettingShot(int hitDirection, Projectile projectile)
    {
        player.OnBeingShot(hitDirection, projectile);
    }

    void GoObjects(bool inDoActive=true)
    {
        // Do game objects
        for (int i = 0; i<gameObjectLength; i++)
        {
            if (!gameObjects[i].FrameEvent())
            {
                gameObjects.RemoveAt(i);
                i--;
                gameObjectLength--;
            }
        }

        // Do enemies
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (!enemyObjects[i].FrameEvent())
            {
                enemyObjects.RemoveAt(i);
                i--;
            }
        }
    }
}