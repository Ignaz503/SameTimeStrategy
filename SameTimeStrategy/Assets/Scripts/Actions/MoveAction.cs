using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : IAction
{
    PlayerController controller;

    bool done = false;
    NavMeshPath path;

    public bool DontEndOnPhaseSwitch
    {
        get
        {
            return true;
        }
    }

    public bool IsPaused { get; protected set; }

    public bool Done
    {
        get
        {
            return done;
        }
    }

    public bool Interruptable
    {
        get
        {
            return true;
        }
    }

    public MoveAction(NavMeshPath path)
    {
        this.path = path;
    }

    public void End()
    {
        controller.Character.Move(Vector3.zero, false, false, false);
        controller.Animator.SetFloat("Forward", 0f);
        controller.Rigidbody.velocity = Vector3.zero;
        controller.Agent.isStopped = true;
        controller.Agent.ResetPath();
        controller.Agent.velocity = Vector3.zero;
        controller.LineRenderer.positionCount = 0;
        controller.LineRenderer.enabled = false;
    }

    public void Start(PlayerController c)
    {
        controller = c;

        //c.Agent.SetDestination(Destination);
        c.Agent.SetPath(path);

        Unpause();//ensuring nothing on conrtoller is in puased state

        //Debug.Log("Starting");
    }

    public void Do()
    {
        //Debug.Log("Doing before pause check");
        if (IsPaused)
            return;

        //Debug.Log("Doing after pause check");
       
        controller.LineRenderer.positionCount = controller.Agent.path.corners.Length;

        controller.LineRenderer.SetPositions(controller.Agent.path.corners);

        if (controller.Agent.remainingDistance > controller.Agent.stoppingDistance)
            controller.Character.Move(controller.Agent.desiredVelocity, false, false, false);
        else if (!done)
        {
            done = true;
            controller.Character.Move(Vector3.zero, false, false);
        }

    }

    public void Pause()
    {
        //Debug.Log("Unpuase");
        IsPaused = true;
    }

    public void Unpause()
    {

        IsPaused = false;
    }

    public void Initialize(PlayerController c)
    {
        controller = c;

        //NavMesh.CalculatePath(controller.transform.position, Destination, -1, path);

        //controller.LineRenderer.positionCount = path.corners.Length;

        //controller.LineRenderer.SetPositions(path.corners);

        controller.LineRenderer.enabled = true;
    }

    public IAction Interrupt(IAction nextAction)
    { 
        MoveFadeOutAction action = new MoveFadeOutAction(nextAction);
        action.Initialize(controller);
        return action;
    }
}