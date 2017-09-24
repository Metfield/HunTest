using UnityEngine;

public class Enemy : GeneralObject
{
    GameObject gameObject;

    public Enemy(Main inMain, int inX, int inY)
    {
        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Enemies/Enemy3_2");

        if (!(gameObject = GameObject.Instantiate(GameObject.Find("Enemy"))))
        {
            main.Trace("Enemy constructor error. Make sure an Enemy is in the scene already");
            return;
        }

        gameObject.transform.parent = gfx.level.transform;
        gameObject.transform.position = new Vector3(x, -y, 1);

        //gameObject = gfx.MakeGameObject("Enemy", sprites[22], x, y);

        SetDirection(-1);
    }

    public override bool FrameEvent()
    {
        // enemy logic here


        // temp logic :)
        //------------------------------------------------------------
        x = x + .4f*direction;
        if ((direction==1 && x > 600) || (direction==-1 && x < 480))
        {
            SetDirection(-direction);
        }
        //------------------------------------------------------------



        UpdatePos();


        return isOK;

    }


    void UpdatePos()
    {
        gfx.SetPos(gameObject, x, y);
    }

    void SetDirection(int inDirection)
    {
        direction = inDirection;
        gfx.SetDirX(gameObject, direction);
    }

    public override void Kill()
    {
       
    }
}