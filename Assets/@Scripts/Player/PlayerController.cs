using System.Linq;
using UnityEngine;
using  UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Field
    [SerializeField] private LayerMask groundLayerMask;
    private float _moveSpeed = 5f;
    private float _jumpForce = 80f;
    private Vector2 _currentMoveInput;
    private Transform _cameraContainerTransform;
    private float _camMinXRotation = -85f;
    private float _camMaxXRotation = 85f;
    private float _camCurrentXRotation;
    private float _rotationSensitive = 0.2f;
    private Vector2 _mouseDelta;
    private bool _IsCanLook = true;
    private Rigidbody _rigidbody;
    private static PlayerController _instance;
    #endregion

    #region Property
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public float JumpForce
    {
        get => _jumpForce;
        set => _jumpForce = value;
    }

    public Transform CameraContainerTransform
    {
        get => _cameraContainerTransform;
        set { _cameraContainerTransform = value; }
    }

    public Vector2 CurrentMoveInput
    {
        get => _currentMoveInput;
        set => _currentMoveInput = value;
    }

    public float CamMinXRotation
    {
        get => _camMinXRotation;
        set => _camMinXRotation = value;
    }

    public float CamMaxXRotation
    {
        get => _camMaxXRotation;
        set => _camMaxXRotation = value;
    }

    public float CamCurrentXRotation
    {
        get => _camCurrentXRotation;
        set => _camCurrentXRotation = value;
    }

    public float RotationSensitive
    {
        get => _rotationSensitive;
        set => _rotationSensitive = value;
    }

    public bool IsCanLook
    {
        get => _IsCanLook;
        set => _IsCanLook = value;
    }
    #endregion

    #region Singleton

    public static PlayerController Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<PlayerController>();
            if (_instance != null)
            {
                DontDestroyOnLoad(_instance.gameObject);
            }
            else
            {
                Debug.LogError("PlayerController instance not found in the scene!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Initialized

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cameraContainerTransform = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion

    #region Update
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (IsCanLook)
        {
            CameraLock();
        }
    }

    #endregion

    #region CameraSetting

    private void CameraLock()
    {
        // 마우스의 X 값을 변형하면 Y의 각이 변함 (고개 끄덕이는 것은 머리가 아닌 목이 움직이는 것과 같음)
        // 오일러 각도에서 Y의 끝 점이 Swing 하기 위해서는 X의 값이 바뀌어야 함
        // 다른예로 어깨를 돌려 바라보는 곳의 방향을 바꾸려면 Y를 돌려 X의 끝점에 대한 변화를 줄 수 있음 

        CamCurrentXRotation += _mouseDelta.y * RotationSensitive;
        CamCurrentXRotation = Mathf.Clamp(CamCurrentXRotation, CamMinXRotation, CamMaxXRotation);
        _cameraContainerTransform.localEulerAngles = new Vector3(-CamCurrentXRotation, 0, 0);
        transform.eulerAngles += new Vector3(0, _mouseDelta.x * RotationSensitive, 0);
    }

    #endregion

    #region Move Action

    private void Move()
    {
        Transform playerTransform = transform;
        Vector3 moveDirection =
            playerTransform.forward * CurrentMoveInput.y + playerTransform.right * CurrentMoveInput.x;
        moveDirection *= MoveSpeed;
        moveDirection.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveDirection;
    }

    #endregion

    #region Movement Events

    public void OnLookInput(InputAction.CallbackContext cbContext)
    {
        _mouseDelta = cbContext.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext cbContext)
    {
        CurrentMoveInput = cbContext.phase switch
        {
            InputActionPhase.Performed => cbContext.ReadValue<Vector2>(),
            InputActionPhase.Canceled => Vector2.zero,
            _ => CurrentMoveInput
        };
    }

    public void OnJumpInput(InputAction.CallbackContext cbContext)
    {
        if (cbContext.phase != InputActionPhase.Started) return;
        if (IsGrounded()) _rigidbody.AddForce(Vector2.up * JumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        Transform playerTransform = transform;
        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;
        Vector3 groundPosition = playerTransform.position;
       
        Ray[] rays = 
        {
           new(groundPosition + forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
           new(groundPosition + -forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
           new(groundPosition + right * 0.2f + Vector3.up * 0.01f, Vector3.down),
           new(groundPosition + -right * 0.2f + Vector3.up * 0.01f ,Vector3.down)
        };

        return rays.Any(t => Physics.Raycast(t, 0.1f, groundLayerMask));
    }

    private void OnDrawGizmos()
    {
        Transform playerTransform = transform;
        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;
        Vector3 groundPosition = playerTransform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundPosition + forward * 0.2f, Vector3.down);
        Gizmos.DrawRay(groundPosition + -forward * 0.2f, Vector3.down);
        Gizmos.DrawRay(groundPosition + right * 0.2f, Vector3.down);
        Gizmos.DrawRay(groundPosition + -right * 0.2f, Vector3.down);
    }
    #endregion
}
