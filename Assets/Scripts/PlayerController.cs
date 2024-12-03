using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{

    private CharacterController _controller;

    private Transform _camera;

    private Animator _animator; 
//-------------InPuts-----------------------------------

    private float _horizontal;

    private float _vertical;

    [SerializeField] private float _movementSpeed = 5;
    
    [SerializeField] private float _pushForce = 10;

    private float _turnSmoothVelocity;
    [SerializeField] private float _turnSmoothTime = 0.5f;

    [SerializeField] private float _jumpHeight = 1;


//----------------Gravedad--------------------------
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;


//----------------Ground Sensor -------------------
    [SerializeField] Transform _sensorPosition;

    [SerializeField] float _sensorRadius = 0.5f;

    [SerializeField] LayerMask _groundLayer;

    private Vector3 moveDirection;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>();
    }
   
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
       
       // Movement();
       

    if(Input.GetButton("Fire2"))
    {
        AimMovement(); //No hacer-lo
    }
    else
    {
        Movement();
    }
    
    
    if(Input.GetButtonDown("Jump") && IsGrounded())
    {
        Jump();
    }
       Gravity();

       if (Input.GetKeyDown(KeyCode.F))
        {
            RayTest();
        }
    }

    

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        
        _animator.SetFloat("VelZ",direction.magnitude);
        _animator.SetFloat("VelX", 0);
    
        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        
            moveDirection = Quaternion.Euler(0, targetAngle, 0)*Vector3.forward;

            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
        
    }

     void AimMovement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ",_vertical);
        _animator.SetFloat("VelX",_horizontal);
        
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref _turnSmoothVelocity, _turnSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
       
       if(direction != Vector3.zero)

       {
       
        moveDirection = Quaternion.Euler(0, targetAngle, 0)*Vector3.forward;

        _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
       }
        
        
    }

    void Gravity()
    {
       if(!IsGrounded())
       {
        _playerGravity.y += _gravity *Time.deltaTime;
       }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = -1;
        }

        _controller.Move(_playerGravity * Time.deltaTime);
        
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    /*bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }*/
    
    bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(_sensorPosition.position, -transform.up, out hit, 2))
        {
            if(hit.transform.gameObject.layer == _groundLayer)
            {
                Debug.DrawRay(_sensorPosition.position, -transform.up * 2, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(_sensorPosition.position, -transform.up * 2, Color.red);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rBody = hit.collider.attachedRigidbody;

        if (rBody != null)
        {
            Vector3 pushDirection = new Vector3 (moveDirection.x, 0, moveDirection.z);
            rBody.velocity = pushDirection * _pushForce / rBody.mass;
        }
    }

    void RayTest()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward,out hit, 10))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.position);
            Debug.Log(hit.transform.gameObject.layer);

            if(hit.transform.gameObject.tag == "Enemy")
            {
                Enemy enemyScrip = hit.transform.gameObject.GetComponent<Enemy>();

                enemyScrip.TakeDamage();
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }
}


