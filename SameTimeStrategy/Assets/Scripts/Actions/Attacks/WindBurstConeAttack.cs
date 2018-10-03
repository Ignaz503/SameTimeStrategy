using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBurstConeAttack : ConeAOEAttack
{
    LayerMask obstacleMask;

    public override bool IsPaused { get; protected set; }

    public override bool DontEndOnPhaseSwitch { get { return true; } protected set{ return; } }
    public override bool Done { get; protected set; }
    public override bool Interruptable { get { return true; } protected set { return; } }

    public WindBurstConeAttack(float angle, float distance, float damage, LayerMask obstacleMask) : base(DamageType.Wind, damage, angle, distance)
    { }

    public override void Do()
    {
        //instant

        //all targets collision inside radius
        Collider[] colliders = Physics.OverlapSphere(attackingCharacter.transform.position, distance);

        for (int i = 0; i < colliders.Length; i++)
        {
            Transform target = colliders[i].transform;

            IDamagable damagable = colliders[i].gameObject.GetComponent<IDamagable>();

            if (target == attackingCharacter.transform || damagable == null)
                continue;

            Vector3 dirToTarget = (target.position - attackingCharacter.transform.position).normalized;

            if (Vector3.Angle(target.forward, dirToTarget) < Angle / 2)
            {
                float distToTarget = Vector3.Distance(attackingCharacter.transform.position, target.position);

                if (!Physics.Raycast(attackingCharacter.transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    //no obstale can see target
                    //damage
                    damagable.DoDamage(type, damage);
                }
            }

        }
        Done = true;
    }

    public override void End()
    {
        //something
    }

    public override void Initialize(PlayerController c)
    {
        attackingCharacter = c;
    }

    public override IAction Interrupt(IAction nextAction)
    {
        return nextAction;
    }

    public override void Pause()
    {
        IsPaused = true;
    }

    public override void Start(PlayerController c)
    {
        // something
    }

    public override void Unpause()
    {
        IsPaused = false;
    }
}
