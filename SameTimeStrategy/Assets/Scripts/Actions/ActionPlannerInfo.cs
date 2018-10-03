using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ActionPlanner")]
public class ActionPlannerInfo : ScriptableObject
{
    public Actions action;

    public Sprite icon;

    public Vector3 iconScale;
    public Vector3 iconRotation;

}
