using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveValueOnCameraMode : MonoBehaviour {

    [Serializable]
    struct ValueModePair
    {
        public CameraModeController.CameraControllMode mode;
        public bool valueEnter;
        public bool valueExit;
    }

    [SerializeField]List<ValueModePair> modeActivePairs;

    private void Start()
    {
        foreach(ValueModePair p in modeActivePairs)
        {
            CameraModeController.Instance.RegisterToEventsForMode(p.mode, () => { gameObject.SetActive(p.valueEnter); }, () => { gameObject.SetActive(p.valueExit); });
        }
    }


}
