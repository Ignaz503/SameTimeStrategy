using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ActionPlanner")]
public class ActionInfo : ScriptableObject
{
    public Actions action;

    public Sprite icon;

    public Vector3 iconScale = Vector3.one;
    public Vector3 iconRotation;
}
