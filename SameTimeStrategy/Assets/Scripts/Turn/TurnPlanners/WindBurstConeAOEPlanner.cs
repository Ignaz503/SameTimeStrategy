using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindBurstConeAOEPlanner : BaseTurnPlaner
{
    LayerMask obstacleMask;
       
    float damageModLerp = .5f;

    readonly float defaulRadius = 5f;
    readonly float defaultAngleInDegree = 90f;

    readonly float defaultDamage = 5f;

    readonly float speed = .1f;

    float minRadius = 1f;
    float minAngleInDegree = 20f;

    float maxRadius = 7f;
    float maxAngleInDegree = 160f;

    float angleInDegree;
    float radius;

    KeyCode dragKey = KeyCode.Mouse0;
    KeyCode angleLock = KeyCode.LeftShift;
    KeyCode radiusLock = KeyCode.LeftControl;

    Vector3 dragStartPos;
    Vector3 dragEndPos;

    GameObject ui;
    GameObject angleSliderObject;
    GameObject radiusSliderObject;

    Slider angleSlider;
    Slider radiusSlider;

    public override void Cancel()
    {
        return;
    }

    public override bool Confirm()
    {
        if(angleInDegree > 0 && radius > 0)
        {
            float dmg = CalcDamage();

            AffectedCharacter.Action = new WindBurstConeAttack(angleInDegree, radius,dmg , obstacleMask);
            AffectedCharacter.Action.Initialize(AffectedCharacter);
            return true;
        }
        return false;
    }

    public override void OnDisable()
    {
        ui?.SetActive(false);
    }

    public override void OnEnable()
    {
        if(angleSlider != null)
            SetAngleSlider();
        if(radiusSlider != null)
            SetRadiusSlider();

        ui?.SetActive(true);
    }

    public float CalcDamage()
    {
        float iTAngle = (Mathf.InverseLerp(maxAngleInDegree, minAngleInDegree, angleInDegree) * 1.5f) - 0.5f;

        float iTRadius = (Mathf.InverseLerp(maxRadius, minRadius, radius) * 1.5f) - 0.5f;

        return defaultDamage + (damageModLerp * (defaultDamage * iTRadius)) + ((1f - damageModLerp) * (defaultDamage * iTAngle));
    }


    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("AOEConeUI");
        angleSliderObject = ui.transform.GetChild(1).gameObject;
        radiusSliderObject = ui.transform.GetChild(3).gameObject;
        ui.transform.GetChild(0).gameObject.SetActive(true);
        ui.transform.GetChild(2).gameObject.SetActive(true);

        angleSliderObject.SetActive(true);
        radiusSliderObject.SetActive(true);

        if (angleSliderObject == null)
            Debug.Log("what");

        angleSlider = angleSliderObject.GetComponent<Slider>();

        SetAngleSlider();

        angleSlider.onValueChanged.AddListener((val) => { angleInDegree = val; Debug.Log(CalcDamage()); });

        radiusSlider = radiusSliderObject.GetComponent<Slider>();

        SetRadiusSlider();

        radiusSlider.onValueChanged.AddListener((val) => { radius = val;});

        //default
        angleInDegree = defaultAngleInDegree;
        radius = defaulRadius;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(AffectedCharacter.transform.position, radius);

        Vector3 dirA = DirectionFromAngle(AffectedCharacter.transform.eulerAngles.y + angleInDegree / 2.0f)* radius;
        Vector3 dirB = DirectionFromAngle(AffectedCharacter.transform.eulerAngles.y - angleInDegree / 2.0f)*radius;

        Gizmos.DrawRay(AffectedCharacter.transform.position, dirA);
        Gizmos.DrawRay(AffectedCharacter.transform.position, dirB);
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool AngleIsGlobal = true)
    {
        if (!AngleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    void SetRadiusSlider()
    {
        radiusSlider.minValue = minRadius;
        radiusSlider.maxValue = maxRadius;

        radiusSlider.value = defaulRadius;
    }

    void SetAngleSlider()
    {
        angleSlider.minValue = minAngleInDegree;
        angleSlider.maxValue = maxAngleInDegree;

        angleSlider.value = defaultAngleInDegree;
    }

}
