using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public event Action Done;

    [SerializeField] Camera cam;
    public Camera Camera { get { return cam; } }

    [SerializeField] LineRenderer lineRenderer;
    public LineRenderer LineRenderer { get { return lineRenderer; } }

    [SerializeField] NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    [SerializeField] ThirdPersonCharacter character;
    public ThirdPersonCharacter Character { get { return character; } }

    [SerializeField] Animator animator;
    public Animator Animator { get { return animator; } }

    [SerializeField] new Rigidbody rigidbody;
    public Rigidbody Rigidbody { get { return rigidbody; } }

    [SerializeField] TurnController turnController;
    public TurnController TurnController { get { return turnController; } }

    [SerializeField] float possibleMovementDistance;

    [SerializeField][Range(1,180)]int stepIncrease = 12;

    IAction _Action;
    public IAction Action
    {
        get { return _Action; }
        set
        {
            if (_Action == null)
                _Action = value;
            else if (_Action.Interruptable)
            {
                _Action.End();// end previous action
                _Action = value;
            }
            else
                Debug.Log("Cannot interrupt current action");
        }
    }

    public List<string> PossibleActions;

    private void Start()
    {
        agent.updateRotation = false;

        turnController.OnPlanningPhaseStart += EndNextAction;

        turnController.OnExecutionPhaseStart += StartNextAction;

        character.OnStopMoving += () => { Debug.Log("Done Moving"); };

    }

    // Update is called once per frame
    void Update()
    {

        if (turnController.IsPlanningPhase)
        {
            UpdatePlanningPhase();
            return;
        }

        if (turnController.IsExecutionPhase)
        {
            UpdateExecutionPhase();
        }

    }

    void UpdatePlanningPhase()
    {
        //For Testing
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray r = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(r, out hit))
        //    {
        //        // move
        //        Action = new MoveAction(hit.point);
        //        Action.Initialize(this);
        //    }
        //}
    }

    void UpdateExecutionPhase()
    {
        if (Action != null && Action.IsPaused)
            Action.Unpause();

        Action?.Do();
    }

    void StartNextAction()
    {
        if (Action != null && Action.IsPaused)
            Action.Unpause();
        else
            Action?.Start(this);
    }

    void EndNextAction()
    {
        if(Action!= null)
        {
            if (Action.DontEndOnPhaseSwitch && !Action.Done)
                Action.Pause();
            else
            {
                Action.End();
                Action = null;
                Done?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        float rot = transform.eulerAngles.y;

        for(float curRot = rot; curRot < rot +360; curRot+= stepIncrease)
        {
            float lerp = Mathf.InverseLerp(rot + 360, rot, curRot);
            lerp = lerp < .5f ? 1.0f - lerp : lerp;

            float dist = possibleMovementDistance * lerp;

            Vector3 dir = new Vector3(Mathf.Sin(curRot*Mathf.Deg2Rad), 0, Mathf.Cos(curRot*Mathf.Deg2Rad)).normalized * dist;

            Gizmos.DrawLine(transform.position, transform.position + dir);

        }
     
    }
}
