using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    void Start(PlayerController c);
    void Initialize(PlayerController c);
    void Do();
    void End();
    void Pause();
    void Unpause();
    bool IsPaused { get; }
    bool DontEndOnPhaseSwitch { get; }
    bool Done { get; }
    bool Interruptable { get; }
}
