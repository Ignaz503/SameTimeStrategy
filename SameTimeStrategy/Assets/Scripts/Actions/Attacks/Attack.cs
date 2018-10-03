using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Piercing,
    Slashing,
    Crushing,
    Fire,
    Water,
    Earth,
    Wind,
    Plasma
}

public abstract class Attack : IAction
{

    protected PlayerController attackingCharacter;
    protected float damage;
    protected DamageType type;
    public DamageType DamageType { get { return type; } }

    protected Attack(DamageType type, float damage)
    {
        this.damage = damage;
        this.type = type;
    }

    public abstract bool IsPaused{get; protected set;}

    public abstract bool DontEndOnPhaseSwitch { get; protected set; }

    public abstract bool Done { get; protected set; }
    public abstract  bool Interruptable { get; protected set; }

    public abstract void Do();

    public abstract void End();

    public abstract void Initialize(PlayerController c);

    public abstract IAction Interrupt(IAction nextAction);

    public abstract void Pause();

    public abstract void Start(PlayerController c);

    public abstract void Unpause();
}
