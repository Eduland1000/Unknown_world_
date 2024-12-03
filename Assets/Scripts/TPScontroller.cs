using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPScontroller : MonoBehaviour
{
   
     private CharacterController _controller;

     private Transform _camera;

     private Transform _lookAtPlayer;

     [SerializeField] private GameObject _normalCamera;

     [SerializeField] private GameObject _aimCamera;
     
    private Animator _animator; 
//-------------InPuts-----------------------------------

    private float _horizontal;

    private float _vertical;

    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;
    
//----------------Gravedad--------------------------
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

//----------------Ground Sensor -------------------
    [SerializeField] Transform _sensorPosition;

    [SerializeField] float _sensorRadius = 0.5f;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] private AxisState xAxis;
    [SerializeField] private AxisState yAxis;
    
 void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _lookAtPlayer = GameObject.Find("Look At Player").transform;
        _animator = GetComponentInChildren<Animator>();

        
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire2"))
        {
            _normalCamera.SetActive(false);
            _aimCamera.SetActive(true);
        }
        else if(Input.GetButtonUp("Fire2"))
        {
            _normalCamera.SetActive(false);
            _aimCamera.SetActive(true);
        }
        
       Movement();

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump(); 
        }

        Gravity();
      
    }

    void Movement()
    {
        Vector3 move = new Vector3 (_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ",_vertical);
        _animator.SetFloat("VelX",_horizontal);

        yAxis.Update(Time.deltaTime);
        xAxis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, xAxis.Value, 0);
        _lookAtPlayer.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0);

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

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
            _animator.SetBool("IsJumping",false);
        }

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        _animator.SetBool("IsJumping",true);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void OnTriggerEnter(Collider other)
{
    // Comprueba si el objeto tocado tiene el tag "Enemy"
    if (other.CompareTag("Enemy"))
    {
        // Reproduce la animación de muerte
        _animator.SetTrigger("ISDEAD");

        // Inicia la corrutina para destruir el personaje tras un pequeño retraso
        StartCoroutine(DelayedDestroy());
    }
}

IEnumerator DelayedDestroy()
{
    // Espera unos segundos para que la animación de muerte se complete
    yield return new WaitForSeconds(4); // Ajusta el tiempo según la duración de tu animación

    // Destruye al jugador
    Destroy(gameObject);
}
}
