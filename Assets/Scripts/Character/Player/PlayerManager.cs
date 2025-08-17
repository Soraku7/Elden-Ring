using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : CharacterManager
{
     [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
     [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    
    protected override void Awake()
    {
        base.Awake();
        
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    protected override void Update()
    {
        base.Update();
        
        if(!IsOwner) return;
        
        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner) return;
        
        base.LateUpdate();
        
        PlayerCamera.instance.HandleAllCameraActions();
    }

    //当对象生成时调用
    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
        }
    }
}
