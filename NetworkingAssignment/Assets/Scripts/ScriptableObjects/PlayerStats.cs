using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    [Range(0f, 10f)] 
    public float               _moveSpeed   = 5f;
    [Range(0f, 100f)] 
    public float               _upwardForce   = 10f;
    [Range(-100f, 0f)] 
    public float               _downwardForce   = -1.0f;
    [Range(0f, 100f)] 
    public float               _jumpHeight   = 20f;
    [SerializeField]
    public Vector3             _playerMovement;

    public Vector3 _playerVelocity;
    
    
    [Header("Mouse Movement")]
    [Range(0f, 10f)] 
    public float               _sensitivity = 5f;
    
    public Vector2             _mouseMovement;
    
    
}
