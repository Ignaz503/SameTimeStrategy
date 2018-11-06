using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateAction : IAction
{
    float targetRotationDegree;
    Vector3 rotateDir;
    float speed;
    float t = 0;

    PlayerController character;


    public RotateAction(float targetRotationDegree)
    {
        this.targetRotationDegree = targetRotationDegree;

        rotateDir = new Vector3(Mathf.Sin(targetRotationDegree * Mathf.Deg2Rad), 0f, Mathf.Cos(targetRotationDegree * Mathf.Deg2Rad));
    }

    public bool IsPaused { get; protected set; }

    public bool DontEndOnPhaseSwitch => true;

    public bool Interruptable => true;

    public bool Done { get; protected set; }

    public void Do()
    {
        if (IsPaused)
            return;

        t += Time.deltaTime * speed;
        if (t >= 1f)
            Done = true;
        else
            character.Character.Rotate(Vector3.Lerp(character.transform.eulerAngles,rotateDir,t));
    }

    public void End()
    {
        Debug.Log("Rotate End");
        character.Character.Move(Vector3.zero, false, false, false);
        character.Animator.SetFloat("Forward", 0f);
        character.Rigidbody.velocity = Vector3.zero;
        character.Agent.isStopped = true;
        character.Agent.ResetPath();
        character.Agent.velocity = Vector3.zero;

    }

    public void Initialize(PlayerController c)
    {
        character = c;
        speed = c.Agent.speed;

    }

    public IAction Interrupt(IAction nextAction)
    {
        return nextAction;
    }

    public void Pause()
    {
        IsPaused = true;

    }

    public void Start(PlayerController c)
    {
        Unpause();
    }

    public void Unpause()
    {
        IsPaused = false;
    }
}
