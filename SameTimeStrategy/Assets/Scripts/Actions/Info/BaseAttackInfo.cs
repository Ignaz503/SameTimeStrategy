using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attack Info")]
public class BaseAttackInfo : ActionInfo
{
    public DamageType DamageType;
    public float maxRange;
    public float minRange;
    public float defualtRange;
}
