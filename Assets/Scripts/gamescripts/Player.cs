using UnityEngine;

public abstract class SpriteCharacter
{
    protected SpriteRenderer spriteRenderer;

    public abstract void UpdatePos();
    public abstract void Kill();
    public abstract void Turn();

    public virtual bool FrameEvent() { return false; }
    public virtual bool FrameEvent(int inMoveX, int inMoveY, bool inShoot) { return false; }
}

public class Player : SpriteCharacter
{
    Main main;
    Game game;
    Gfx  gfx;
    Snd  snd;

    Sprite[] sprites;

    GameObject gameObject;
    Vector3 playerPosition;

    

    float x, previousDx;
    float y;

    public Player (Main inMain, bool startFacingRight)
    {
        main = inMain;
        game = main.game;
        gfx  = main.gfx;
        snd  = main.snd;

        sprites = gfx.GetLevelSprites("Players/Player1");

        x = 370;
        y = 624;

        // We're using the Unity engine, let's use prefabs :D
        if(!(gameObject = GameObject.Instantiate(GameObject.Find("Player"))))
        {
            main.Trace("Player constructor error. Make sure a Player is in the scene already");
            return;
        }

        gameObject.transform.parent = gfx.level.transform;
        gameObject.transform.position = new Vector3(x, -y, 1);

        playerPosition = gameObject.transform.localPosition;

        if (startFacingRight)
            previousDx = 1;
        else
            previousDx = -1;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inShoot)
    {
        // Player logic here


        // temp logic :)
        //------------------------------------------------------------

        

        // Flip sprite accordingly
        if (previousDx != inMoveX)


        x = x + inMoveX;
        previousDx = inMoveX;


        if (inMoveY == -1)
            Jump();

        if (inMoveY == 1)
            Duck();
        //------------------------------------------------------------



        UpdatePos();

        if (inShoot) {
            snd.PlayAudioClip("Gun");
        }

    }

    void Jump()
    {
        main.Trace("Player::Jump!");
    }

    void Duck()
    {
        main.Trace("Player::Duck!"); 
    }

    protected override void UpdatePos()
    {
        playerPosition.x = x;
        playerPosition.y = -y;

        gameObject.transform.localPosition = playerPosition;
    }

   






}