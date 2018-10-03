using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurnPlaner : BaseTurnPlaner
{

    [SerializeField] KeyCode rotationDragKey = KeyCode.Mouse0;
    Camera playerCamera;

    float targetRotation;
    bool rotIsSet = false;

    private void Start()
    {
        AffectedCharacter.RotationVisualizer.enabled = true;
    }


    private void Update()
    {
        if (Input.GetKey(rotationDragKey))
        {
            Vector3 screenMiddle = playerCamera.WorldToScreenPoint(AffectedCharacter.transform.position);

            Vector3 pos = Input.mousePosition - screenMiddle;

            targetRotation = Mathf.Atan2(pos.x, pos.y) * Mathf.Rad2Deg;

            targetRotation = FitAngleInto0to360(targetRotation);


            targetRotation += (playerCamera.transform.eulerAngles.y - AffectedCharacter.transform.eulerAngles.y);

            targetRotation = FitAngleInto0to360(targetRotation);

            rotIsSet = true;

            float percent = targetRotation / 360f;

            //Debug.Log($"angle:{angle} percent: {percent}");
            targetRotation = AffectedCharacter.transform.eulerAngles.y + targetRotation;

            if (percent <= .5f)
            {
                AffectedCharacter.RotationVisualizer.fillAmount = percent;
                AffectedCharacter.RotationVisualizer.fillClockwise = true;
            }
            else
            {
                AffectedCharacter.RotationVisualizer.fillAmount = 1.0f - percent;
                AffectedCharacter.RotationVisualizer.fillClockwise = false;
            }

        }
    }

    float FitAngleInto0to360(float angle)
    {
        angle = angle < 0 ? 360 + angle : angle;
        angle = angle > 360 ? angle - 360 : angle;
        return angle;
    }


    public override void Cancel()
    {
        AffectedCharacter.RotationVisualizer.fillAmount = 0f;
        AffectedCharacter.RotationVisualizer.enabled = false;
    }

    public override bool Confirm()
    {
        //Debug.Log(targetRotation - AffectedCharacter.transform.eulerAngles.y);

        if (rotIsSet)
        {
            AffectedCharacter.RotationVisualizer.enabled = false;
            AffectedCharacter.Action = new RotateAction(targetRotation);
            AffectedCharacter.Action.Initialize(AffectedCharacter);
            return true;
        }
        return false;
    }

    public override void OnDisable()
    {

    }

    public override void OnEnable()
    {
        rotIsSet = false;
        playerCamera = Camera.main;
        if(AffectedCharacter != null)
            AffectedCharacter.RotationVisualizer.enabled = true;

    }

}
