using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;


[RequireComponent((typeof(CharacterController)))]
public class PlayerController : MonoBehaviour {
    
    private InputActionAsset inputActionAsset;
    
    [Header("Player Settings")]
    [SerializeField]
    private PlayerStats _playerStats;
    
    [Header("Player Camera")]
    [SerializeField]
    private Transform _playerCamera;
    
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

    private void OnEnable() {
        _inputSystem.Player.Enable();
        _inputSystem.UI.Enable();
    }

    private void OnDisable() {
        _inputSystem.Player.Disable();
        _inputSystem.UI.Disable();
    }

    void Start() {
        InitializePlayerMovement();
    }

    void Update()
    {
        _inputSystem.Player.Jump.performed += ctx => OnJumpPressed();
        Jump();
        Movement();
        Look();
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
            _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
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
    // private bool IsGrounded() {
    //     
    //     if (Physics.Raycast(transform.position, Vector3.down, out _hit, 0.6f, layerMask)) {
    //         Debug.DrawRay(transform.position, Vector3.down, Color.red);
    //         return true;
    //     }
    //     else {
    //         Debug.DrawRay(transform.position, Vector3.down, Color.white);
    //         return false;
    //     }
    // }
    
}
