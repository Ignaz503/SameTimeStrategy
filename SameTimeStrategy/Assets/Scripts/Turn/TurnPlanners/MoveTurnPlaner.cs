using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTurnPlaner : BaseTurnPlaner
{
   
    [SerializeField] KeyCode clickKey = KeyCode.Mouse0;
    [SerializeField] Camera raycastCamera;

    bool destinationHasBeenSet;
    NavMeshPath destination;

    public override void Cancel()
    {
       //nothing
    }

    public override bool Confirm()
    {
        if(destinationHasBeenSet)
        { 
            AffectedCharacter.Action = new MoveAction(destination);
            AffectedCharacter.Action.Initialize(AffectedCharacter);
            return true;
        }
        return false;
    }

    public override void OnDisable()
    {

    }

    public override void OnEnable()
    {
        //use to do setup beofre affected char is set
        destination = new NavMeshPath();
        raycastCamera = Camera.main;
    }

    private void Start()
    {
        //use to do setup after affected char is set
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = raycastCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                destinationHasBeenSet = NavMesh.CalculatePath(AffectedCharacter.transform.position,hit.point,-1,destination);

                AffectedCharacter.LineRenderer.positionCount = destination.corners.Length;

                AffectedCharacter.LineRenderer.SetPositions(destination.corners);

                AffectedCharacter.LineRenderer.enabled = true;
            }
        }
    }

}
