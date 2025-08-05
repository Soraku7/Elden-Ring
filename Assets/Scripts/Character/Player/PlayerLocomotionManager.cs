using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager _player;
    
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 _moveDirection;
    [SerializeField] private float walkingSpeed = 2.0f;
    [SerializeField] private float runningSpeed = 5f;
    
    protected override void Awake()
    {
        base.Awake();
        
        _player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
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
            _player.characterController.Move(_moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            _player.characterController.Move(_moveDirection * walkingSpeed * Time.deltaTime);
        }
    }
}
