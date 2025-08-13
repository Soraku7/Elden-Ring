using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager _player;
    
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 _moveDirection;
    private Vector3 _targetRotationDirection;
    [SerializeField] private float walkingSpeed = 2.0f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    
    protected override void Awake()
    {
        base.Awake();
        
        _player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
    }

    private void HandleGroundedMovement()
    {
        GetVerticalHorizontalInputs();
        
        _moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        _moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        if (PlayerInputManager.instance.moveAmount > 0.5f)
        {
            _player.characterController.Move(_moveDirection * (runningSpeed * Time.deltaTime));
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            _player.characterController.Move(_moveDirection * (walkingSpeed * Time.deltaTime));
        }
    }
    
    private void HandleRotation()
    {
        _targetRotationDirection = Vector3.zero;
        _targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        _targetRotationDirection = _targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        
        //防止摄像机移动时改变玩家移动过上下朝向
        _targetRotationDirection.Normalize();
        _targetRotationDirection.y = 0;
        
        if(_targetRotationDirection == Vector3.zero) _targetRotationDirection = transform.forward;

        Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation , newRotation , rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
