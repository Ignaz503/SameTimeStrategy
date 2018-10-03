using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFadeOutAction : FadeOutAction
{

    public MoveFadeOutAction(IAction nextAction) : base(nextAction)
    {
        Done = false;
    }

    public override void Do()
    {
        if (IsPaused)
            return;

        Done = true;
    }

    public override void Initialize(PlayerController c)
    {
        base.Initialize(c);
        affectedCharacter.LineRenderer.enabled = false;
        affectedCharacter.Agent.ResetPath();
    }

    public override void Pause()
    {
        IsPaused = true;

        affectedCharacter.Character.enabled = false;
        affectedCharacter.Animator.enabled = false;
        affectedCharacter.Rigidbody.Sleep();
        affectedCharacter.Agent.isStopped = true;
    }

    public override void Start(PlayerController c)
    {
        Unpause();
    }

    public override void Unpause()
    {

        IsPaused = false;

        affectedCharacter.Agent.isStopped = false;
        affectedCharacter.Rigidbody.WakeUp();
        affectedCharacter.Animator.enabled = true;
        affectedCharacter.Character.enabled = true;
    }

    public override void End()
    {
        affectedCharacter.Character.Move(Vector3.zero, false, false, false);
        affectedCharacter.Animator.SetFloat("Forward", 0f);
        affectedCharacter.Rigidbody.velocity = Vector3.zero;
        affectedCharacter.Agent.isStopped = true;
        affectedCharacter.Agent.ResetPath();
        affectedCharacter.Agent.velocity = Vector3.zero;
        affectedCharacter.LineRenderer.positionCount = 0;
        affectedCharacter.LineRenderer.enabled = false;

        base.End();
    }

}
