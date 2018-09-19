using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCameraController : MonoBehaviour {

    [Serializable]
    public class CameraPositionRotation
    {
        [SerializeField] public Vector3 Position;
        [SerializeField] public Vector3 Rotation;
    }

    [Serializable]
    public class CameraLevel
    {
        [SerializeField]public List<CameraPositionRotation> levelSettings;
    }

    [SerializeField]List<CameraLevel> viewPositions;
    [SerializeField] Transform cameraToControll;

    [SerializeField] int currentLevel;
    [SerializeField] int currentPosition;

    [SerializeField] KeyCode upALevel = KeyCode.W;
    [SerializeField] KeyCode downALevel = KeyCode.S;
    [SerializeField] KeyCode left = KeyCode.A;
    [SerializeField] KeyCode right = KeyCode.D;

    [SerializeField] float movementSmoothtime = .5f;
    [SerializeField] float rotationSmoothtime = .5f;

    CameraPositionRotation currentTarget;
    Vector3 velPos;
    Vector3 velRot;

    private void Awake()
    {
        foreach (CameraLevel lvl in viewPositions)
            if (lvl.levelSettings.Count == 0)
                throw new Exception("Can't have camera level with 0 positions");
    }

    private void Start()
    {
        currentTarget = viewPositions[viewPositions.Count - 1].levelSettings[0];

    }

    private void OnEnable()
    {
        float minDist = float.PositiveInfinity;

        for (int level = 0; level < viewPositions.Count; level++)
        {
            CameraLevel lvl = viewPositions[level];
            for (int pos = 0; pos < lvl.levelSettings.Count; pos++)
            {
                CameraPositionRotation posRot = lvl.levelSettings[pos];
                float dist = (posRot.Position - cameraToControll.transform.position).sqrMagnitude;
                if ( dist < minDist)
                {
                    minDist = dist;
                    currentTarget = posRot;

                    currentLevel = level;
                    currentPosition = pos;
                   
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(upALevel))
        {
            currentLevel = (currentLevel + 1) % viewPositions.Count;
        }

        if (Input.GetKeyDown(downALevel))
        {
            currentLevel--;

            currentLevel = currentLevel < 0 ? viewPositions.Count - 1 : currentLevel; 
        }

        if (Input.GetKeyDown(left))
        {
            currentPosition = (currentPosition + 1) % viewPositions[currentLevel].levelSettings.Count;
        }

        if (Input.GetKeyDown(right))
        {
            currentPosition--;

            currentPosition = currentPosition < 0 ? viewPositions[currentLevel].levelSettings.Count - 1 : currentPosition;
        }

        CameraLevel curLevSet = viewPositions[currentLevel];

        if (curLevSet.levelSettings.Count <= currentPosition)
            currentPosition = currentPosition - (currentPosition - (curLevSet.levelSettings.Count - 1));

        currentTarget = curLevSet.levelSettings[currentPosition];
    }

    private void LateUpdate()
    {
        cameraToControll.transform.position = Vector3.SmoothDamp(cameraToControll.transform.position, currentTarget.Position, ref velPos, movementSmoothtime);

        cameraToControll.transform.rotation = Quaternion.RotateTowards(cameraToControll.transform.rotation, Quaternion.Euler(currentTarget.Rotation), rotationSmoothtime * Time.deltaTime);
    }


}
