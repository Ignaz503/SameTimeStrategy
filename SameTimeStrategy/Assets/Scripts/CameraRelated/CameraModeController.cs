using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class CameraModeController : MonoBehaviour {

    public enum CameraControllMode
    {
        Free,
        Static,
        Count//don't use used for modulo operation shoud always be last element
    }

    public event Action OnFreeModeEnter; 
    public event Action OnFreeModeExit;
    public event Action OnStaticModeEnter;
    public event Action OnStaticModeExit;

    public static CameraModeController Instance;

    [SerializeField] KeyCode switchMode = KeyCode.Tab;

    [SerializeField] GameObject crossHair;

    [SerializeField] StaticCameraController staticController;
    [SerializeField] PlayerFreeMove freeController;

    CameraControllMode _CurrentMode;
    public CameraControllMode CurrentMode
    {
        get { return _CurrentMode; }
        protected set
        {
            _CurrentMode = value;
            switch (_CurrentMode)
            {
                case CameraControllMode.Free:
                    OnStaticModeExit?.Invoke();
                    OnFreeModeEnter?.Invoke();
                    break;
                case CameraControllMode.Static:
                    OnFreeModeExit?.Invoke();
                    OnStaticModeEnter?.Invoke();
                    break;
                default:
                    throw new Exception($"Mode set to unrecognised state");
            }
        }
    }


    private void Awake()
    {
        if (Instance != null)
            throw new Exception("There already exists a camera mode controller");
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        CurrentMode = CameraControllMode.Static;
        staticController.enabled = true;
        freeController.enabled = false;

        OnStaticModeEnter += () => staticController.enabled = true;
        OnStaticModeExit += () => staticController.enabled = false;

        OnFreeModeEnter += () => freeController.enabled = true;
        OnFreeModeExit += () => freeController.enabled = false;
        OnFreeModeEnter += () => crossHair.SetActive(true);
        OnFreeModeExit += () => crossHair.SetActive(false);


    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(switchMode))
        {
            CurrentMode = (CameraControllMode)(((int)CurrentMode +1 )%(int)CameraControllMode.Count);
        }


	}

    public void RegisterToEventsForMode(CameraControllMode mode, Action enter, Action exit)
    {
        switch(mode)
        {
            case CameraControllMode.Free:
                OnFreeModeEnter += enter;
                OnFreeModeExit += exit;
                break;
            case CameraControllMode.Static:
                OnStaticModeEnter += enter;
                OnStaticModeEnter += exit;
                break;
            default:
                throw new Exception("Cannot register to non exitent mode");
        }
    }

    public void UnRegisterFromEventsForMode(CameraControllMode mode, Action enter, Action exit)
    {
        switch (mode)
        {
            case CameraControllMode.Free:
                OnFreeModeEnter -= enter;
                OnFreeModeExit -= exit;
                break;
            case CameraControllMode.Static:
                OnStaticModeEnter -= enter;
                OnStaticModeEnter -= exit;
                break;
            default:
                throw new Exception("Cannot register to non exitent mode");
        }
    }

}
