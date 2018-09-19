using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTurnPlaner : MonoBehaviour
{
    public PlayerController AffectedCharacter { get; set; }

    public abstract void OnEnable();

    public abstract void OnDisable();

    public abstract bool Confirm();

    public abstract void Cancel();

}
