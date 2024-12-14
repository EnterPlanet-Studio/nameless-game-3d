using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_mp : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private float _horizontalInput, _verticalInput;
    [SerializeField]
    private float _moveSpeed, _rotationSpeed, _jumpForce;
    private Quaternion _targetRotation;

    [SerializeField]
    private LayerMask _whatIsGround;
    private bool _isGrounded, _readyToJump = true;

    [SerializeField]
    private string _horizontalAxis, _verticalAxis;

    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private mp_trash_spawner _trashManager;

    [Header("Input System")]
    private PlayerInput _playerInput;
    private InputAction _moveAction, _rotateAction, _jumpAction;

    private int _trash;
    [SerializeField]
    private TMPro.TMP_Text _trashText;

    [SerializeField]
    private GameObject _win;

    void Start()
    {
        /*_targetRotation = transform.rotation;*/
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.currentActionMap.FindAction("Move");
        _rotateAction = _playerInput.currentActionMap.FindAction("Rotate");
        _jumpAction = _playerInput.currentActionMap.FindAction("Jump");

        _playerInput.SwitchCurrentControlScheme(Keyboard.current);
    }

    // Update is called once per frame
    void Update()
    {
        if (_trash >= 5) {
            _win.SetActive(true);
            Time.timeScale = 0f;
        }
        _isGrounded = Physics.Raycast(
            _rigidbody.position + new Vector3(0, 0.1f, 0), 
            Vector3.down, 
            0.5f, 
            _whatIsGround
        );
        _camera.position = new Vector3(transform.position.x + 3f, 6.5f, transform.position.z - 4f);
        MyInput();
        //MoveRotation();
        SpeedControl();
    }
    void FixedUpdate() {
        MovePlayer();
        
    }
    private void MyInput() {
        _horizontalInput = _rotateAction.ReadValue<float>();
        _verticalInput = _moveAction.ReadValue<float>();
        _moveDirection = new Vector3(_moveSpeed*_horizontalInput, 0, _moveSpeed*_verticalInput);

        /*if (_horizontalInput != 0) {
            float angle = _horizontalInput * _rotationSpeed * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            _targetRotation *= rotation;
        }*/
        if (_horizontalInput != 0 || _verticalInput != 0) {
            Vector3 dir = new Vector3(-_rigidbody.velocity.x, 0, -_rigidbody.velocity.z);
            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = rotation;
        }

        if (_jumpAction.WasPressedThisFrame() && _isGrounded && _readyToJump)
            Jump();
    }
    private void Jump() {
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _readyToJump = false;
        Invoke("ResetJump", 0.5f);
    }
    private void ResetJump() {
        _readyToJump = true;
    }
    private void MovePlayer()
    {
        _rigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
    }
    /*void MoveRotation() {
        if (_targetRotation != transform.rotation) {
            Quaternion rotation = _targetRotation;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                rotation,
                _rotationSpeed
            );
        }
    }*/
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized* _moveSpeed;
            _rigidbody.velocity = new Vector3(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
        }

    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("garbage")) {
            _trash++;
            _trashManager.Give();
            _trashText.text = _trash.ToString();
        }
    }
}