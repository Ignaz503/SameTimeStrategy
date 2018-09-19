using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlannerController : MonoBehaviour
{
    [SerializeField] TurnController turnController;
    [SerializeField] BaseTurnPlaner[] turnPlanners;
    [SerializeField] PlayerController affectedCharacter;
    [SerializeField] CameraModeController cameraModeController;

    BaseTurnPlaner currentlyActivePlanner;
    GameObject radialMenu;

    private void Start()
    {
        turnController.OnExecutionPhaseStart += DisableAllPlanners;
    }

    private void Update()
    {
        
    }

    public void EnablePlanner(int idx)
    {
        currentlyActivePlanner?.Cancel();
        for (int i = 0; i < turnPlanners.Length; i++)
        {
            if (i == idx)
            {
                turnPlanners[i].AffectedCharacter = affectedCharacter;
                turnPlanners[i].enabled = true;
                currentlyActivePlanner = turnPlanners[i];
            }
            else
                turnPlanners[i].enabled = false;
        }

    }

    public void DisableAllPlanners()
    {
        EnablePlanner(-1);
        currentlyActivePlanner = null;
    }

    public void Confirm()
    {
        currentlyActivePlanner?.Confirm();
    }

    void CreateRadialMenuForPlayer()
    {

    }

    void SetPlayerController(PlayerController c)
    {
        affectedCharacter = c;
        CreateRadialMenuForPlayer();
    }

}
