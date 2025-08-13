using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    
    private PlayerControl _playerControl;

    [Header("Movement Input")]
    [SerializeField] private Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;

    [Header("Camera Input")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;
    
    public float moveAmount;

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
        
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false; 
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_playerControl == null)
        {
            _playerControl = new PlayerControl();
            
            _playerControl.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>(); 
            _playerControl.PlayerCamera.CameraControls.performed += i => cameraInput =i.ReadValue<Vector2>();
        }
        
        _playerControl.Enable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleCameraInput();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (enabled)
        {
            if (hasFocus)
            {
                _playerControl.Enable();
            }
            else
            {
                _playerControl.Disable();
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    
    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5f && moveAmount <= 1f)
        {
            moveAmount = 1f;
        }
    }

    private void HandleCameraInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }
}
