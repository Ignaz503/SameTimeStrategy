using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPlannerController : MonoBehaviour
{
    [Serializable]
    public class ActionButton
    {
        public Button Button;
        public Image buttonImage;
        public Image ActionIcon;
        public bool toggle;//toggle for button

        Color c;

        public void Start()
        {
            c = buttonImage.color;
            toggle = false;
        }

        public void Disable()
        {
            buttonImage.color = c;
            toggle = false;
        }

        public void Enable()
        {
            buttonImage.color = Button.colors.pressedColor;
            toggle = true;
        }

    }



    [SerializeField] Sprite NoActionAvailableIcon;

    [SerializeField] TurnController turnController;

    [SerializeField] PlayerController affectedCharacter;
    [SerializeField] CameraModeController cameraModeController;
    [SerializeField] ActionButton[] turnPlannerStartButtons;

    [SerializeField] KeyCode nextTurnKey = KeyCode.Space;

    Tuple<BaseTurnPlaner,ActionButton> currentlyActivePlanner;
    GameObject radialMenu;

    private void Start()
    {
        turnController.OnExecutionPhaseStart += DisableAllTurnPlanStarters;
        turnController.OnPlanningPhaseStart += EnableAllTurnPlanStarters;

        foreach (ActionButton b in turnPlannerStartButtons)
            b.Start();

        SetPlayerController(affectedCharacter);//TEMP
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextTurnKey))
        {
            if (!Confirm())
                Debug.Log("Give command master warning");

            turnController.StartExecutionPhase = true;
            currentlyActivePlanner?.Item1?.Disable();
            currentlyActivePlanner?.Item2?.Disable();
            currentlyActivePlanner = null;
        }
        
    }

    public void EnablePlanner(int idx, BaseTurnPlaner b)
    {
        currentlyActivePlanner?.Item1.Cancel();
        currentlyActivePlanner?.Item1.Disable();
        currentlyActivePlanner?.Item2.Disable();

        currentlyActivePlanner = new Tuple<BaseTurnPlaner, ActionButton>(b, turnPlannerStartButtons[idx]);
        b.Enable();
        turnPlannerStartButtons[idx].Enable();
    }

    public void DisableAllTurnPlanStarters()
    {
        foreach (ActionButton b in turnPlannerStartButtons)
            b.Button.gameObject.SetActive(false);

        currentlyActivePlanner?.Item1?.Disable();
        currentlyActivePlanner?.Item2?.Disable();
    }

    public void EnableAllTurnPlanStarters()
    {
        foreach (ActionButton b in turnPlannerStartButtons)
            b.Button.gameObject.SetActive(true);
    }

    public bool Confirm()
    {
        if(currentlyActivePlanner != null)
        {
            if (currentlyActivePlanner.Item1 != null)
                return currentlyActivePlanner.Item1.Confirm();
        }
        return false;
    }

    void CreateMenuForPlayer()
    {
        for (int i = 0; i < turnPlannerStartButtons.Length; i++)
        {
            //destroy old one
            Destroy(turnPlannerStartButtons[i].Button.gameObject.GetComponent<BaseTurnPlaner>());

            if(affectedCharacter.PossibleActions.Length <= i || affectedCharacter.PossibleActions[i] == null)
            {
                turnPlannerStartButtons[i].ActionIcon.sprite = NoActionAvailableIcon;
                turnPlannerStartButtons[i].ActionIcon.color = Color.red;
                
            } else {

                turnPlannerStartButtons[i].ActionIcon.sprite = affectedCharacter.PossibleActions[i].icon;

                turnPlannerStartButtons[i].ActionIcon.color = Color.blue;
                turnPlannerStartButtons[i].ActionIcon.transform.localScale = affectedCharacter.PossibleActions[i].iconScale;
                turnPlannerStartButtons[i].ActionIcon.transform.localEulerAngles = affectedCharacter.PossibleActions[i].iconRotation;

                BaseTurnPlaner b = ActionPlannerFactory.BuildActionPlanner(affectedCharacter.PossibleActions[i].action, turnPlannerStartButtons[i].Button.gameObject);

                b.AffectedCharacter = affectedCharacter;
                b.Disable();
                int idx = i;// index for button
                
                turnPlannerStartButtons[i].Button.onClick.AddListener(() => 
                {
                    if (!turnPlannerStartButtons[idx].toggle)
                    {
                        EnablePlanner(idx, b);
                    }
                    else
                    {
                        b.Disable();
                        turnPlannerStartButtons[idx].Disable();
                    }
                });

            }

        }
    }

    void SetPlayerController(PlayerController c)
    {
        affectedCharacter = c;
        CreateMenuForPlayer();
    }

}
