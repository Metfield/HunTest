using UnityEngine;

public class Player {

    Main main;
    Game game;
    Gfx  gfx;
    Snd  snd;

    Sprite[] sprites;

    GameObject gameObject;
    Vector3 playerPosition;

    float x;
    float y;

    CharacterAnimator animator;

    public Player (Main inMain) {

        main = inMain;
        game = main.game;
        gfx  = main.gfx;
        snd  = main.snd;

        sprites = gfx.GetLevelSprites("Players/Player1");

        x = 370;
        y = 624;

        // We're using the Unity engine, let's use prefabs :D
        gameObject = GameObject.Find("Player");
        gameObject.transform.parent = gfx.level.transform;
        gameObject.transform.position = new Vector3(x, -y, 1);

        //gameObject = gfx.MakeGameObject("Player", sprites[22], x, y,"Player");
        playerPosition = gameObject.transform.localPosition;
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inShoot) {


        // Player logic here


        // temp logic :)
        //------------------------------------------------------------
        x = x + inMoveX;
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

    void UpdatePos() {

        playerPosition.x = x;
        playerPosition.y = -y;

        gameObject.transform.localPosition = playerPosition;
    }

   






}