using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTurnPlaner : BaseTurnPlaner
{
    [SerializeField] Transform clickVisualizer;
    [SerializeField] KeyCode clickKey;
    [SerializeField] Camera raycastCamera;

    bool destinationHasBeenSet;
    Vector3 destination;

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
        clickVisualizer.gameObject.SetActive(false);
    }

    public override void OnEnable()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = raycastCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                destinationHasBeenSet = true;
                destination = hit.point;
                clickVisualizer.position = hit.point;
                clickVisualizer.gameObject.SetActive(true);
            }
        }
    }

}
