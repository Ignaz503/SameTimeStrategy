using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFreeMove : MonoBehaviour {

    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;

    [SerializeField] Vector2 yClampValues;

    [SerializeField] private MouseLook m_MouseLook;

    [SerializeField] KeyCode IncreaseY = KeyCode.LeftControl;
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;

    [SerializeField] TurnController turnController;

    private Camera m_Camera;

    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    Vector3 vel;

    // Use this for initialization
    void Start ()
    {
        m_Camera = Camera.main;

        transform.eulerAngles = Vector3.zero;
        m_MouseLook.Init(transform, m_Camera.transform);

        CameraModeController.Instance.OnFreeModeExit += () => m_Camera.transform.localEulerAngles = Vector3.zero;
        CameraModeController.Instance.OnFreeModeExit += () => m_MouseLook.SetCursorLock(false);
        CameraModeController.Instance.OnStaticModeExit += () => m_MouseLook.SetCursorLock(true);
    }
	
	// Update is called once per frame
	void Update () {
        RotateView();
        Move();
    }


    private void Move()
    {
        float speed;
        GetInput(out speed);

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = (m_Camera.transform.forward * m_Input.y + transform.right * m_Input.x);

        if (Input.GetKey(IncreaseY))
            desiredMove.y += 1f;

        m_MoveDir = desiredMove * speed;

        transform.position += m_MoveDir * Time.unscaledDeltaTime; 

        m_MouseLook.UpdateCursorLock();

    }

    private void LateUpdate()
    {
        //TODO y clamp   
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(runKey);
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);
        if(Time.timeScale == 0f)
            Debug.Log($"input: {m_Input}");

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

    }

    private void OnEnable()
    {
        if (m_Camera != null)
        {
            transform.eulerAngles = Vector3.zero;
            m_MouseLook.Init(transform, m_Camera.transform);
        }


    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }


}
