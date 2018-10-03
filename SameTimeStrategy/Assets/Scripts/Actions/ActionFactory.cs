using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class ActionPlannerFactory {

    public static BaseTurnPlaner BuildActionPlanner(Actions action, GameObject container)
    {
        switch (action)
        {
            case Actions.Rotate:
                //container.AddComponent
                return container.AddComponent<RotateTurnPlaner>();
            case Actions.Move:
                return container.AddComponent<MoveTurnPlaner>();
            case Actions.WindBurstConeAttack:
                return container.AddComponent <WindBurstConeAOEPlanner>(); ;
            default:
                return null;
        }// end switch
    }
}
