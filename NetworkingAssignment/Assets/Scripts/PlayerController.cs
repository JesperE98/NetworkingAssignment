using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;


[RequireComponent((typeof(CharacterController)))]
public class PlayerController : NetworkBehaviour {
    
    private Spawner spawner;
    private InputActionAsset inputActionAsset;
    
    [Header("Player Settings")]
    [SerializeField]
    private PlayerStats _playerStats;
    [SerializeField]
    private Transform _respawnPoint;
    [SerializeField] private float  _fallThreshhold = -10f;
    
    private                  Camera _playerCamera;
    
    private CharacterController _characterController;
    private InputSystem_Actions _inputSystem;
    private float               _xRotation;
    
    private RaycastHit _hit;
    [SerializeField]
    private LayerMask  layerMask;

    private bool _groundedPlayer;
    private bool _jumpedPressed = false;
    private float            _gravityValue = -9.81f;
    
    private void Awake() {
        _inputSystem         = new InputSystem_Actions();
        _characterController = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn() {
        spawner = GameObject.Find("SpawnManager").GetComponent<Spawner>();
        _inputSystem.Player.Enable();
        _inputSystem.UI.Enable();
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn() {
        _inputSystem.Player.Disable();
        _inputSystem.UI.Disable();
        
        base.OnNetworkDespawn();
    }

    void Start() {
        InitializePlayerMovement();
    }

    void Update()
    {
        _inputSystem.Player.Jump.performed += ctx => OnJumpPressed();
        _inputSystem.Player.Ping.performed += ctx => {
                                                  spawner.PingServerRpc(new Vector3(transform.localPosition.x,
                                                                            transform.localPosition.y + 1,
                                                                            transform.localPosition.z));
                                              };

        // Respawns the player if they fall
        if (IsOwner && transform.position.y < _fallThreshhold) {
            TriggerRespawnServerRpc();
        }
        
        Jump();
        Movement();
        Look();
        
    }

    [ServerRpc]
    private void TriggerRespawnServerRpc() {
        RespawnPlayer();
    }

    private void RespawnPlayer() {
        if (!IsServer) return;
        
        RespawnTrigger respawnPoint = new RespawnTrigger();
        
        transform.position = respawnPoint.transform.position;
    }

    private void InitializePlayerMovement() {
        _inputSystem.Player.Move.performed += ctx => _playerStats._playerMovement = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
        _inputSystem.Player.Move.canceled += ctx => _playerStats._playerMovement = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);

       
        _inputSystem.Player.Look.performed += ctx => _playerStats._mouseMovement = ctx.ReadValue<Vector2>();
        _inputSystem.Player.Look.canceled  += ctx => _playerStats._mouseMovement              = ctx.ReadValue<Vector2>();
    }

    private void Movement() {

        Vector3 moveVec = transform.TransformDirection(_playerStats._playerMovement);
        
        _characterController.Move(moveVec * _playerStats._moveSpeed * Time.deltaTime);
    }

    private void Look() {

        if (Mouse.current.rightButton.isPressed) {
            Vector2 NonNormalizedDelta = _playerStats._mouseMovement * 0.5f * 0.1f;
        
            _xRotation -= NonNormalizedDelta.y * _playerStats._sensitivity;
        
            transform.Rotate(0f, NonNormalizedDelta.x * _playerStats._sensitivity, 0f);
            _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
    
    private void Jump() {
        _groundedPlayer = _characterController.isGrounded;

        if (_groundedPlayer) {
            _playerStats._playerVelocity.y = 0.0f;
        }

        if (_jumpedPressed && _groundedPlayer) {
            _playerStats._playerVelocity.y += Mathf.Sqrt(_playerStats._jumpHeight * -2.0f * _gravityValue);
            _jumpedPressed                 =  false;
        }
        
        _playerStats._playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerStats._playerVelocity * Time.deltaTime);
    }

    void OnJumpPressed() {

        if (_characterController.velocity.y == 0) {
            _jumpedPressed = true;
        }
    }
}
