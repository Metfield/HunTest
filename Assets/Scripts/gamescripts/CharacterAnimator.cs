using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Gfx gfx;
    GameObject characterObject;

    

    // Animation frames (inclusive)
    private AnimationWindow idle, run, shoot, 
                            jump, duck, shotFromBehind, 
                            shotFromFront;

    public enum Stance
    {
        Idle,
        Run,
        Shoot,
        Jump,
        Duck,
        ShotFromBehind,
        ShotFromFront
    }

    public CharacterAnimator(Gfx gfx, GameObject characterObject)
    {
        this.gfx = gfx;
        this.characterObject = characterObject;
    }

    public void Play(Stance type, int speed)
    {
        switch (type)
        {
            case Stance.Idle:
                Play(idle);
                break;
            case Stance.Run:
                Play(run);
                break;
            case Stance.Shoot:
                Play(shoot);
                break;
            case Stance.Jump:
                Play(jump);
                break;
            case Stance.Duck:
                Play(duck);
                break;
            case Stance.ShotFromBehind:
                Play(shotFromBehind);
                break;
            case Stance.ShotFromFront:
                Play(shotFromFront);
                break;
        }
    }

    void Play(AnimationWindow aw)
    {
        //gfx.SetSprite(characterObject, )
    }

    void SetIdleAnimationFrames(int start, int end)
    {
        idle.start = start;
        idle.end = end;
    }

    void SetRunAnimationFrames(int start, int end)
    {
        run.start = start;
        run.end = end;
    }

    void SetShootAnimationFrames(int start, int end)
    {
        shoot.start = start;
        shoot.end = end;
    }

    void SetJumpAnimationFrames(int start, int end)
    {
        jump.start = start;
        jump.end = end;
    }

    void SetDuckAnimationFrames(int start, int end)
    {
        duck.start = start;
        duck.end = end;
    }

    void SetShotFromBehindAnimationFrames(int start, int end)
    {
        shotFromBehind.start = start;
        shotFromBehind.end = end;
    }

    void SetShotInFrontAnimationFrames(int start, int end)
    {
        shotFromFront.start = start;
        shotFromFront.end = end;
    }
}
