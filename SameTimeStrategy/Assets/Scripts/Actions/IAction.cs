using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actions
{
    Move,
    Rotate,
    WindBurstConeAttack
}

public interface IAction
{
    void Start(PlayerController c);
    void Initialize(PlayerController c);
    void Do();
    void End();
    void Pause();
    void Unpause();
    IAction Interrupt(IAction nectAction);
    bool IsPaused { get; }
    bool DontEndOnPhaseSwitch { get; }
    bool Done { get; }
    bool Interruptable { get; }
}
