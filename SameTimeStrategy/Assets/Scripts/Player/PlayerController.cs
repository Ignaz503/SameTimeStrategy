using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public event Action Done;

    [SerializeField] LineRenderer lineRenderer;
    public LineRenderer LineRenderer { get { return lineRenderer; } }

    [SerializeField] Image rotationVisualizer;
    public Image RotationVisualizer { get { return rotationVisualizer; } }

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

    [SerializeField] [Range(1, 180)] int stepIncrease = 12;

    IAction _Action;
    public IAction Action
    {
        get { return _Action; }
        set
        {
            if (_Action == null)
                _Action = value;
            else if(_Action.Done)
            {
                if(!(_Action is FadeOutAction))_Action.End();// end previous action , if in here to avoid enless loop with fade action setting next action in End function
                _Action = value;
            }
            else if (_Action.Interruptable)
            {
                Debug.Log("Interrutp");
                _Action = _Action.Interrupt(value);
            }
            else
                Debug.Log("Cannot chage current action");
        }
    }

    public ActionPlannerInfo[] PossibleActions;

    private void Start()
    {
        agent.updateRotation = false;

        turnController.OnPlanningPhaseStart += EndNextAction;

        turnController.OnExecutionPhaseStart += StartNextAction;

        //character.OnStopMoving += () => { Debug.Log("Done Moving"); };


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

    }

    void UpdateExecutionPhase()
    {
        if(Action != null)
        {
            if (Action.IsPaused)
                Action.Unpause();
            else if (Action.Done)
            {
                Action.End();
                return;
            }
            Action?.Do();
        }
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

        //float rot = transform.eulerAngles.y;

        //for(float curRot = rot; curRot < rot +360; curRot+= stepIncrease)
        //{
        //    float lerp = Mathf.InverseLerp(rot + 360, rot, curRot);
        //    lerp = lerp < .5f ? 1.0f - lerp : lerp;

        //    float dist = possibleMovementDistance * lerp;

        //    Vector3 dir = new Vector3(Mathf.Sin(curRot*Mathf.Deg2Rad), 0, Mathf.Cos(curRot*Mathf.Deg2Rad)).normalized * dist;

        //    Gizmos.DrawLine(transform.position, transform.position + dir);

        //}


        Gizmos.DrawLine(transform.position, (transform.position + transform.forward));
    }
}
