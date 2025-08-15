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
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;
    
    [Header("Camera Values")]
    private Vector3 _cameraVelocity;
    private Vector3 _cameraObjectPosition;

    [SerializeField] private float leftAndRightLookingAngle;
    [SerializeField] private float upAndDownLookingAngle;
    private float _cameraZPosition;
    private float _targetCameraZPosition;

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
        _cameraZPosition = cameraObject.transform.localPosition.z;
    }
    
    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowPlayer();
            HandleRotations();
            HandleCollisions();
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
    
    
    private void HandleCollisions()
    {
        _targetCameraZPosition = _cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //拿到摄像机发出射线前方物体
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit,
                Mathf.Abs(_targetCameraZPosition) , collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            _targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }
        
        //判断物体距离是否超过默认摄像机位置
        if(Mathf.Abs(_targetCameraZPosition) < _cameraZPosition)
        {
            _targetCameraZPosition = -_cameraZPosition;
        }
        
        _cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = _cameraObjectPosition;
    }
}