using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    
    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] private Transform cameraPivotTransform;
    
    [Header("Camera Settings")]
    private float _cameraSmoothSpeed = 1f;
    [SerializeField] private float leftAndRightRotationSpeed = 220f;
    [SerializeField] private float upAndDownRotationSpeed = 220f;
    [SerializeField] private float minimumPivot = -30f;
    [SerializeField] private float maximumPivot = 60f;
    
    [Header("Camera Values")]
    private Vector3 _cameraVelocity;

    [SerializeField] private float leftAndRightLookingAngle;
    [SerializeField] private float upAndDownLookingAngle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowPlayer();
            HandleRotations();
        }
    }

    private void HandleFollowPlayer()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref _cameraVelocity,
            _cameraSmoothSpeed * Time.deltaTime);
        
        transform.position = targetPosition;
    }

    private void HandleRotations()
    {
        leftAndRightLookingAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        upAndDownLookingAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        
        upAndDownLookingAngle = Mathf.Clamp(upAndDownLookingAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        
        cameraRotation.y = leftAndRightLookingAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;
        
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookingAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }
}