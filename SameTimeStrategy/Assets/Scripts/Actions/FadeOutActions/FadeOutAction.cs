using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FadeOutAction : IAction
{

    public IAction actionAfterFadeOut;

    protected PlayerController affectedCharacter;

    public bool IsPaused { get; protected set; }

    public bool DontEndOnPhaseSwitch => true;

    public bool Done { get; protected set; }

    public bool Interruptable => false;

    protected FadeOutAction(IAction actionAfterFadeOut)
    {
        this.actionAfterFadeOut = actionAfterFadeOut;
    }

    public abstract void Do();

    public virtual void End()
    {
        //Debug.Log("FadeOut end");
        affectedCharacter.Action = actionAfterFadeOut;
        actionAfterFadeOut.Initialize(affectedCharacter);
        actionAfterFadeOut.Start(affectedCharacter);
    }

    public virtual void Initialize(PlayerController c)
    {
        affectedCharacter = c;
    }

    public abstract void Pause();

    public abstract void Start(PlayerController c);

    public abstract void Unpause();

    public void SetActionAfterFadeOut(IAction nextAction)
    {
        actionAfterFadeOut = nextAction;
    }

    public IAction Interrupt(IAction nectAction)
    {
        throw new System.Exception("Trying to interrupt non interruptable action");
    }
}
