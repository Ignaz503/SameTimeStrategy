using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : IAction
{
    PlayerController controller;
    public Vector3 Destination { get; set; }
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

    public MoveAction(Vector3 destination)
    {
        Destination = destination;
        path = new NavMeshPath();   
    }

    public void End()
    {
        controller.Character.Move(Vector3.zero, false, false, false);
        controller.Animator.SetFloat("Forward", 0f);
        controller.Rigidbody.velocity = Vector3.zero;
        controller.Agent.isStopped = true;
        controller.Agent.ResetPath();
        controller.Agent.velocity = Vector3.zero;
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

        controller.Character.enabled = false;
        controller.Animator.enabled = false;
        controller.Rigidbody.Sleep();
        controller.Agent.isStopped = true;
    }

    public void Unpause()
    {
        Debug.Log("Unpuase");
        IsPaused = false;

        controller.Character.enabled = true;
        controller.Animator.enabled = true;
        controller.Rigidbody.WakeUp();
        controller.Agent.isStopped = false;
    }

    public void Initialize(PlayerController c)
    {
        controller = c;

        NavMesh.CalculatePath(controller.transform.position, Destination, -1, path);

        controller.LineRenderer.positionCount = path.corners.Length;

        controller.LineRenderer.SetPositions(path.corners);

        controller.LineRenderer.enabled = true;
    }
}