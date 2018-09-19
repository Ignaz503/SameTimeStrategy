using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnController : MonoBehaviour
{
    public enum GamePhase
    {
        PlanningPhase,
        ExecutionPhase
    }

    [SerializeField] [Range(.5f, 5f)] float roundTime;
    public float RoundTime { get { return roundTime; } }
    [SerializeField] KeyCode nexTurnKey = KeyCode.Space;
    float currentTime = 0f;

    [SerializeField]float plannigPhaseTimeMultiplier = 1f;
    public float PlannigPhaseTimeMultiplier { get { return plannigPhaseTimeMultiplier; } }

    [SerializeField] float executionTimeMultiplier = 1f;
    public float ExecutionTimeMultiplier { get { return executionTimeMultiplier; } }

    public GamePhase _Phase;
    public GamePhase Phase
    {
        get { return _Phase; }
        protected set
        {
            GamePhase old = _Phase;
            _Phase = value;
            phaseDisplay.text = _Phase.ToString();
            if(old != value)
            {
                //phase changed
                OnPhaseChange?.Invoke(old, value);
            }
        }
    }

    public bool IsExecutionPhase { get { return Phase == GamePhase.ExecutionPhase; } }
    public bool IsPlanningPhase { get { return Phase == GamePhase.PlanningPhase; } }

    [SerializeField] Image clock;
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] TextMeshProUGUI phaseDisplay;

    public Action OnExecutionPhaseStart;
    public Action OnPlanningPhaseStart;
    /// <summary>
    /// func(oldPhase, newPhase)
    /// </summary>
    public Action<GamePhase,GamePhase> OnPhaseChange;
    
    private void Awake()
    {
        //currentTime = RoundTime;
        Phase = GamePhase.PlanningPhase;
    }

    // Update is called once per frame
    void Update () {

        if (currentTime <= 0)
            UpdatePhase();
        else
            UpdateCurrentTime();//update current time
	}

    void UpdateCurrentTime()
    {
        currentTime -= Time.deltaTime;

        clock.fillAmount = (currentTime / roundTime);
        clockText.text = string.Format("{0:0.00}", currentTime < 0 ? 0f:currentTime);
        clockText.color = Color.Lerp(Color.red, Color.green, currentTime / roundTime);
    }

    void UpdatePhase()
    {
        //we already now currentTime is less or equal zero
        if (IsExecutionPhase)
        {
            Phase = GamePhase.PlanningPhase;
            OnPlanningPhaseStart?.Invoke();
            clockText.text = "0.00";
            Time.timeScale = PlannigPhaseTimeMultiplier;
        }
        if (IsPlanningPhase && Input.GetKeyDown(nexTurnKey))
        {
            currentTime = roundTime;
            Time.timeScale = ExecutionTimeMultiplier;
            Phase = GamePhase.ExecutionPhase;
            OnExecutionPhaseStart?.Invoke();
        }// end if next turn key
    }
}
