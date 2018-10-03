using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConeAOEAttack : Attack
{
    protected float angle;
    public float Angle { get { return angle; } }

    protected float distance;
    public float Distance { get { return distance; } }

    public ConeAOEAttack(DamageType type, float dmage, float angle, float distance): base(type,dmage)
    {
        Done = false;
        this.angle = angle;
        this.distance = distance;
    }

}
