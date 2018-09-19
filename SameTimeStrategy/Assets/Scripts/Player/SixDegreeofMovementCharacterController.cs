using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class SixDegreeofMovementCharacterController : MonoBehaviour
{
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;

    [SerializeField] private MouseLook m_MouseLook;

    [SerializeField] KeyCode IncreaseY;

    [SerializeField] TurnController turnController;

    private Camera m_Camera;

    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;


    // Use this for initialization
    private void Start()
    {
        
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;

        m_CharacterController.detectCollisions = false;

        transform.eulerAngles = Vector3.zero;
        m_MouseLook.Init(transform, m_Camera.transform);

        CameraModeController.Instance.OnFreeModeExit += () => m_Camera.transform.localEulerAngles = Vector3.zero; 
        CameraModeController.Instance.OnFreeModeExit += () => m_MouseLook.SetCursorLock(false);
        CameraModeController.Instance.OnStaticModeExit += () => m_MouseLook.SetCursorLock(true);

    }

    // Update is called once per frame
    private void Update()
    {
        RotateView();
        // the jump state needs to read here to make sure it is not missed
    }


    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = (m_Camera.transform.forward * m_Input.y + transform.right * m_Input.x);

        if (Input.GetKey(IncreaseY))
            desiredMove.y += 1f;

        m_MoveDir = desiredMove * speed;


        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedUnscaledDeltaTime);

        m_MouseLook.UpdateCursorLock();
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

    }

    private void OnEnable()
    {
        if(m_Camera != null)
        {
            transform.eulerAngles = Vector3.zero;
            m_MouseLook.Init(transform, m_Camera.transform);
        }

        
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Rigidbody body = hit.collider.attachedRigidbody;
        ////dont move the rigidbody if the character is on top of it
        //if (m_CollisionFlags == CollisionFlags.Below)
        //{
        //    return;
        //}

        //if (body == null || body.isKinematic)
        //{
        //    return;
        //}
        //body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
